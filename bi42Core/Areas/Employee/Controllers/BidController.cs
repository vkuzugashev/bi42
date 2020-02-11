using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Bi42.Models;
using System.Web.Security;
using System.Web;

namespace bi42.Areas.Employer.Controllers
{
    public class BidController : Controller
    {
        private DbModel db = new DbModel();


        // GET: /Employer/Project/OrderBids
        [Authorize]
        public ActionResult Index()
        {
            string UserID = User.Identity.Name;
            Profile profile = db.Profiles.SingleOrDefault(p => p.UserID == UserID && p.ProfileTypeID == 2 && p.IsActive == true);
            if (profile == null)
            {
                //Нет профиля перенаправил на вкладку профилей
                return RedirectToAction("Index", "Profile");
            }
            IList<Bid> bids = db.Bids.Where(x => x.Status == 0 && x.UserID == UserID && x.Order.OrderAreaID == profile.OrderAreaID).ToList<Bid>();
            return View(bids);
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BidCreate(int orderid = 0)
        {
            Order order = db.Orders.SingleOrDefault(p => p.OrderID == orderid);
            if (order == null)
            {
                return HttpNotFound();
            }
            Bid model = new Bid { Order=order, OrderID=order.OrderID };
            return View(model);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BidCreate()
        {
            Bid bid = new Bid();
            if (TryUpdateModel<Bid>(bid, new string[] { "OrderID", "CostTotal", "TimeRate", "Description" })) 
            {
                Order order = db.Orders.SingleOrDefault(p => p.OrderID == bid.OrderID);
                if (order == null)
                {
                    return HttpNotFound();
                }

                string UserID = User.Identity.Name;
                Profile profile = db.Profiles.SingleOrDefault(p => p.UserID == UserID && p.ProfileTypeID == 2 && p.IsActive == true);
                bid.Order = order;
                bid.Profile = profile;
                bid.PublishTime = DateTime.Now;
                bid.UserID = UserID;
                bid.BidStatusID = 1;
                db.Bids.Add(bid);
                //Удалим проект из избранных
                OrderWatch projectWatch = db.OrderWatches.SingleOrDefault(p => p.OrderID == bid.OrderID && p.UserID == UserID);
                if (projectWatch != null)
                {
                    projectWatch.OrderWatchStatusID = 2;  //Сделана ставка
                    db.Entry(projectWatch).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            return RedirectToAction("Details", "OrderWatch", new { orderid=bid.OrderID });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BidUpdate(int id = 0)
        {
            string UserID = User.Identity.Name;
            Bid bid = db.Bids.SingleOrDefault(x => x.BidID == id && x.UserID==UserID);
            if (bid == null)
            {
                return HttpNotFound();
            }
            return View(bid);
        }

        // POST: /Employer/Order/OrderBid/5
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BidUpdate(int id, bool? bidDel, bool? bidAccept)
        {
            string UserID = User.Identity.Name;
            Bid bidnew = new Bid();
            if (TryUpdateModel<Bid>(bidnew))
            {
                Bid bid = db.Bids.First<Bid>(x => x.BidID == id);

                if (bid == null)
                {
                    return HttpNotFound();
                }

                Order Order = db.Orders.SingleOrDefault(p => p.OrderID == bid.OrderID);

                if (bid.BidStatusID == 1 && bidDel == true)
                    db.Bids.Remove(bid);
                else
                {
                    if (bid.BidStatusID == 1)//Обновление предложения пока не выбран
                    {
                        bid.CostTotal = bidnew.CostTotal;
                        bid.TimeRate = bidnew.TimeRate;
                        bid.Description = bidnew.Description;
                    }
                    else
                    {
                        if (bidAccept == true)
                        {
                            bid.BidStatusID = 3;//Заключён контракт
                            Order.OrderStatusID = 3;//Заключён контракт
                            //todo сделать заключение контракта работником
                            Contract contract = db.Contracts.FirstOrDefault<Contract>(x => x.OrderID == bid.OrderID && x.BidID == bid.BidID);
                            if(contract!=null)
                                contract.AcceptTime = DateTime.Now;
                        }
                        else
                        {
                            bid.BidStatusID = 6;//Отказ подрядчика
                            //todo Если более нет подрядчиков то статус проекта изменить?
                        }
                        bid.ResponseTime = DateTime.Now;//Дата ответа
                    }
                    db.Entry(bid).State = EntityState.Modified;
                    db.Entry(Order).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult BidDetails(int id = 0)
        {
            Bid bid = db.Bids.SingleOrDefault(x => x.BidID == id);
            if (bid == null)
            {
                return HttpNotFound();
            }
            var lastAccess = db.GetLastAccess("emp_pub_msg_" + bid.OrderID, User.Identity.Name);
            if (lastAccess != null)
                ViewBag.NewPublicMessages = db.PublicOrderMessages.Count(x => x.PublishTime > lastAccess.LastAccess && x.OrderID == bid.OrderID) ;
            lastAccess = db.GetLastAccess("emp_prv_msg_" + bid.OrderID, User.Identity.Name);
            if (lastAccess != null)
                ViewBag.NewPrivateMessages = db.PrivateOrderMessages.Count(x => x.PublishTime > lastAccess.LastAccess && x.OrderID == bid.OrderID);
            lastAccess = db.GetLastAccess("emp_order_task_" + bid.OrderID, User.Identity.Name);
            if (lastAccess != null)
                ViewBag.NewOrderTasks = db.OrderTasks.Count(x => x.PublishTime > lastAccess.LastAccess && x.OrderID == bid.OrderID);

            return View(bid);
        }


        [Authorize]
        public ActionResult BidDelete(int id = 0)
        {
            string UserID = User.Identity.Name;
            Bid bid = db.Bids.Single(p => p.BidID == id && p.UserID == UserID);
            if (bid == null)
            {
                return HttpNotFound();
            }
            return View(bid);
        }


        [HttpPost, ActionName("BidDelete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            string UserID = User.Identity.Name;
            Bid bid = db.Bids.Single(p => p.BidID == id && p.UserID == UserID);
            if (bid == null)
            {
                return HttpNotFound();
            }

            //Перенести в архив если нет контрактов или статус проекта завершён
            if (bid.BidStatusID != 3 || bid.OrderStatusID != 3)
            {
                bid.Status = 1; //Архивный статус
                foreach(OrderWatch ow in db.OrderWatches.Where(x => x.OrderID == bid.OrderID).ToList())
                {
                    db.OrderWatches.Remove(ow);
                }
            }
            else
            {
                //Нельзя удалить проект с заключёнными контрактами
                ViewBag.Message = "Нельзя удалить/закрыть заявку с незавершённым контрактом! Сначала надо расторгнуть контракт!";
                return View("Delete");
            }
            
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}