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
    public class BidController : Controller
    {
        private DbModel db = new DbModel();


        [Authorize]
        public ActionResult Index(int orderid)
        {
            string UserID = User.Identity.Name;
            Order order = db.Orders.First<Order>(x => x.OrderID == orderid);
            ViewBag.OrderID = order.OrderID;
            ViewBag.OrderName = order.Name;
            ViewBag.OrderDescription = order.Description;
            IList<Bid> bids = db.Bids.Where(x => x.OrderID == orderid).ToList<Bid>();
            return View(bids);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BidView(int orderid, int id = 0)
        {
            string UserID = User.Identity.Name;
            Bid bid = db.Bids.SingleOrDefault(x => x.BidID == id);
            if (bid == null)
            {
                return HttpNotFound();
            }
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"];
            if (TempData["ContractText"] != null)
                ViewBag.ContractText = TempData["ContractText"];
            else
            {
                Contract contract = db.Contracts.FirstOrDefault(x => x.OrderID == bid.OrderID && x.BidID == bid.BidID);
                if (contract != null)
                {
                    ViewBag.ContractID = contract.ContractID;
                    ViewBag.ContractText = contract.Description;
                    if (contract.PublishTime!=null)
                        ViewBag.PublishTime = contract.PublishTime;
                    if (contract.AcceptTime != null)
                        ViewBag.AcceptTime = contract.AcceptTime;
                }
            }
            return View(bid);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BidAccept(int BidID = 0, bool isAgree = false)
        {
                string UserID = User.Identity.Name;
                Bid bid = db.Bids.SingleOrDefault(x => x.BidID == BidID);
                if (bid == null)
                {
                    return HttpNotFound();
                }
                if (isAgree && Request.Params["ContractText"] != null && !"".Equals(Request.Params["ContractText"]))
                {
                    bid.BidStatusID = 2;    //Ставка выбрана
                    bid.AcceptTime = DateTime.Now;
                    //todo сделать заключение контракта
                    Contract contract = db.Contracts.SingleOrDefault<Contract>(x => x.OrderID == bid.OrderID && x.BidID == bid.BidID);
                    if (contract == null)
                    {
                        //Создадим новый контракт
                        contract = new Contract
                        {
                            BidID = bid.BidID,
                            OrderID = bid.OrderID,
                            PublishTime = DateTime.Now,
                            Description = Request.Params["ContractText"]
                        };
                        db.Contracts.Add(contract);
                    } else
                    {
                        //Обновим текст контракта
                        contract.PublishTime = DateTime.Now;
                        contract.Description = Request.Params["ContractText"];
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", new { orderid = bid.OrderID });
                }
                else
                {
                    TempData["Message"] = "Необходимо принять заявку исполнителя и заполнить текст контракта!";
                    TempData["ContractText"] = Request.Params["ContractText"];
                    return RedirectToAction("BidView", new { orderid=bid.OrderID, id=bid.BidID });
                }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}