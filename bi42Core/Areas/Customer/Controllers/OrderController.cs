using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bi42.Models;
using System.IO;
using System.Web.Security;


namespace bi42.Areas.Customer.Controllers
{
    public class OrderController : Controller
    {
        private DbModel db = new DbModel();

        //
        // GET: /Customer/Order/
        [Authorize]
        public ActionResult Index()
        {
            IList<Order> model = new List<Order>();
            string UserID = User.Identity.Name;
            Profile profile = db.Profiles.SingleOrDefault(p => p.UserID == UserID && p.ProfileTypeID==1 && p.IsActive == true);
            if (profile != null)
                model = db.Orders.Include("Profile").Include("BudgetLevel").Where(x => x.Status==0 && x.UserID == UserID && x.ProfileID == profile.ProfileID).ToList<Order>();
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"];
            return View(model);
        }

        //
        // GET: /Customer/Order/Details/5
        [Authorize]
        public ActionResult Details(int orderid = 0)
        {
            Order order = db.Orders.SingleOrDefault(p => p.OrderID == orderid);
            if (order == null)
            {
                return HttpNotFound();
            }
            var lastAccess = db.GetLastAccess("cstm_pub_msg_" + orderid, User.Identity.Name);
            if(lastAccess != null)
                ViewBag.NewPublicMessages = db.PublicOrderMessages.Count(x => x.PublishTime > lastAccess.LastAccess && x.OrderID == orderid);
            lastAccess = db.GetLastAccess("cstm_prv_msg_" + orderid, User.Identity.Name);
            if (lastAccess != null)
                ViewBag.NewPrivateMessages = db.PrivateOrderMessages.Count(x => x.PublishTime > lastAccess.LastAccess && x.OrderID == orderid);
            lastAccess = db.GetLastAccess("cstm_order_task_" + orderid, User.Identity.Name);
            if (lastAccess != null)
                ViewBag.NewOrderTasks = db.OrderTasks.Count(x => x.PublishTime > lastAccess.LastAccess && x.OrderID == orderid);
            return View(order);
        }

        //
        // GET: /Customer/Order/Create

        [Authorize]
        public ActionResult Create()
        {
            string UserID = User.Identity.Name;
            Profile profile = db.Profiles.SingleOrDefault(p => p.UserID == UserID && p.ProfileTypeID==1 && p.IsActive == true);
            //Если нет активного профиля то и нельза создать заказ
            if (profile == null)
            {
                TempData["Message"] = "Нельзя создать заказ без активного профиля!";
                return RedirectToAction("Index");
            }
            ViewBag.OrderAreaID = new SelectList(db.OrderAreas, "OrderAreaID", "Name");
            ViewBag.BudgetLevelID = new SelectList(db.BudgetLevels, "BudgetLevelID", "Name");
            ViewBag.OrderStatusID = new SelectList(db.OrderStatuses.Where(x=> x.OrderStatusID < 3), "OrderStatusID", "Name");
            return View(new Order());
        }

        //
        // POST: /Customer/Order/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(Order order, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string dir = Server.MapPath("~/Content/Profiles/Profile"+order.ProfileID+"/Orders");
                    if (!System.IO.Directory.Exists(dir))
                        System.IO.Directory.CreateDirectory(dir);

                    string filename = "Order" + order.OrderID + ".zip";
                    var path = Path.Combine(dir, filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    file.SaveAs(path);
                    order.OrderFile = "/Content/Profiles/Profile" + order.ProfileID + "/" + filename;
                }
                string UserID = User.Identity.Name;
                order.UserID = UserID;
                order.Status = 0; //Архивный статус
                order.ProfileID = db.Profiles.Single(p => p.UserID == UserID && p.ProfileTypeID==1 && p.IsActive == true).ProfileID;
                //Order.Profile = db.Profiles.Single(p => p.UserID == UserID && p.ProfileTypeID == 1 && p.IsActive == true);
                if (order.PublishTime == null && order.OrderStatusID == 2)
                    order.PublishTime = DateTime.Now;
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        [Authorize]
        public ActionResult Edit(int orderid = 0)
        {
            string UserID = User.Identity.Name;
            Order order = null;
            if (orderid == 0 || (order = db.Orders.Single(p => p.OrderID == orderid && p.UserID == UserID))==null)
            {
                return HttpNotFound();
            }
            //Запретить редактировать если статус более чем черновик
            if (order.OrderStatusID > 1)
                return RedirectToAction("Details", new { orderid=orderid });
            ViewBag.BudgetLevelID = new SelectList(db.BudgetLevels, "BudgetLevelID", "Name", order.BudgetLevelID);
            IList<OrderStatus> OrderStatuses = null;
            if (order.PublishTime == null && order.OrderStatusID < 3)
                OrderStatuses = db.OrderStatuses.Where(x => x.OrderStatusID < 3).ToList<OrderStatus>();
            else
                OrderStatuses = db.OrderStatuses.Where(x => x.OrderStatusID > 1).ToList<OrderStatus>();
            ViewBag.OrderAreaID = new SelectList(db.OrderAreas, "OrderAreaID", "Name");
            ViewBag.OrderStatusID = new SelectList(OrderStatuses, "OrderStatusID", "Name", order.OrderStatusID);

            return View(order);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int orderid, bool? delAttach, HttpPostedFileBase file)
        {
            string UserID = User.Identity.Name;
            Order order = db.Orders.Single(p => p.OrderID == orderid && p.UserID == UserID);
            if (TryUpdateModel<Order>(order))
            {
                string url = "~/Content/Profiles/" + order.ProfileID + "/Orders/" + orderid;
                string dir = Server.MapPath(url);
                string filename = "order" + orderid;
                SharedLib.UploadFile(dir, filename, delAttach, file);

                if (delAttach == true)
                {
                    order.OrderFile = null;
                    order.ContentType = null;
                    order.FileName = null;
                }
                if (file != null && file.ContentLength > 0)
                {
                    order.OrderFile = url + "/" + filename;
                    order.ContentType = file.ContentType;
                    order.FileName = file.FileName;
                }

                if (order.PublishTime == null && order.OrderStatusID == 2)
                    order.PublishTime = DateTime.Now;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BudgetLevelID = new SelectList(db.BudgetLevels, "BudgetLevelID", "Name", order.BudgetLevelID);
            return View(order);
        }


        //
        // GET: /Customer/Order/Cancel/5
        [HttpGet]
        [Authorize]
        public ActionResult Cancel(int orderid = 0)
        {
            string UserID = User.Identity.Name;
            Order order = null;
            if (orderid == 0 || (order = db.Orders.Single(p => p.OrderID == orderid && p.UserID == UserID)) == null)
            {
                return HttpNotFound();
            }
            //Запретить отмену если статус более чем дендер
            if (order.OrderStatusID > 2)
                return RedirectToAction("Details", new { id = orderid });

            return View(order);
        }

        //
        // POST: /Customer/Order/cancel/5

        [HttpGet]
        [Authorize]
        public ActionResult CancelConfirmed(int orderid)
        {
            string UserID = User.Identity.Name;
            Order order = db.Orders.Single(p => p.OrderID == orderid && p.UserID == UserID);
            order.OrderStatusID = 5;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        //
        // GET: /Customer/Order/Delete/5

        [Authorize]
        public ActionResult Delete(int orderid = 0)
        {
            string UserID = User.Identity.Name;
            Order order = db.Orders.Single(p => p.OrderID == orderid && p.UserID == UserID);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // POST: /Customer/Profile/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int orderid)
        {
            string UserID = User.Identity.Name;
            Order order = db.Orders.Single(p => p.OrderID == orderid && p.UserID == UserID);
            if (order.Bids.Count > 0)
            {
                //Перенести в архив если нет контрактов или статус проекта завершён
                //Если проект в статусе Тендер, предложение контракта, запрос на закрытие
                if (order.OrderStatusID == 1 || order.OrderStatusID == 2 || order.OrderStatusID == 5 || order.Bids.Count(x=>x.BidStatusID == 3) == 0)
                {
                    order.Status = 1; //Архивный статус
                    order.OrderStatusID = 6;
                    foreach (Bid bid in order.Bids)
                    {
                        bid.Status = 1;    //Архивный статус
                        db.Entry(bid).State = EntityState.Modified;
                    }
                    foreach (Contract contract in order.Contracts)
                    {
                        contract.Status = 1;    //Архивный статус
                        contract.FinishTime = DateTime.Now;
                        db.Entry(contract).State = EntityState.Modified;
                    }
                }
                else
                {
                    //Нельзя удалить проект с заключёнными контрактами
                    ViewBag.Message = "Нельзя удалить/закрыть заказ с незавершёнными контрактами! Работник должен запросить закрытие заказа в связи с его выполнением!";
                    return View("Delete");
                }
                ////Order.OrderStatusID = 4;    //Завершён
                //foreach (Bid bid in Order.Bids)
                //{
                //    bid.BidStatusID = 5;    //Отказ заказчика
                //    bid.Status = 1;    //Архивный статус
                //    db.Entry(bid).State = EntityState.Modified;
                //}
                
            }
            else
            {
                //Ставок нет удаляем заказ безвозвратно
                order.OrderWatches.ToList().ForEach(row => order.OrderWatches.Remove(row));
                db.Orders.Remove(order);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public FileStreamResult GetOrderFile(int id)
        {
            Order order;
            order = db.Orders.Find(id);

            if (order == null)
                return null;

            string filePath = Server.MapPath(order.OrderFile);
            if (System.IO.File.Exists(filePath))
            {
                Response.Clear();
                Response.ContentType = order.ContentType;
                Response.AddHeader("content-disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(order.FileName) + "\"");
                return File(new FileStream(filePath, FileMode.Open), order.ContentType);
            }
            else
                return null;
        }

             

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}