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
    public class ContractController : Controller
    {
        private DbModel db = new DbModel();

        [Authorize]
        [HttpGet]
        public ActionResult ContractText(int orderid, int b)
        {
            string UserID = User.Identity.Name;
            Contract contract = db.Contracts.FirstOrDefault(x => x.OrderID == orderid && x.BidID == b);
            if (contract == null)
            {
                return HttpNotFound();
            }
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"];
            return View(contract);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ContractText(int ContractID, bool? accept)
        {
            Contract contract = db.Contracts.Find(ContractID);
                
            if (TryUpdateModel<Contract>(contract))
            {
                if (accept == true)
                {
                    string UserID = User.Identity.Name;

                    if (contract.AcceptTime == null)
                        contract.AcceptTime = DateTime.Now;
                    contract.Bid = db.Bids.Find(contract.BidID);
                    contract.Order = db.Orders.Find(contract.OrderID);
                    contract.Bid.BidStatusID = 3;//Заключён контракт
                    contract.Bid.ResponseTime = DateTime.Now;
                    contract.Order.OrderStatusID = 3;//Заключён контракт
                    if (contract.ContractID == 0)
                        db.Contracts.Add(contract);
                    else
                        db.Entry(contract).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("BidDetails", "Bid", new { id = contract.BidID });
                }
                else
                    TempData["Message"] = "Выберите пункт \"Я согласен\"!";

                return RedirectToAction("ContractText", new { id = contract.OrderID, b = contract.BidID });
            }
            else
            {
                TempData["Message"] = "Возникла ошибка.";
            }
            return RedirectToAction("ContractText", new { id = contract.OrderID, b = contract.BidID });
        }

        [Authorize]
        [HttpGet]
        public ActionResult ContractReport(int orderid, int b)
        {
            string UserID = User.Identity.Name;
            Contract contract = db.Contracts.FirstOrDefault(x => x.OrderID == orderid && x.BidID == b);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ContractReport(int ContractID, string Report, bool? accept)
        {
            Contract contract = db.Contracts.Find(ContractID);
            if (contract!=null)
            {
                if (accept == true && Report!=null && !"".Equals(Report.Trim()))
                {
                    string UserID = User.Identity.Name;
                    contract.Report = Report;
                    if (contract.AcceptTime == null)
                        contract.AcceptTime = DateTime.Now;
                    contract.ReportTime = DateTime.Now;
                    contract.Order.OrderStatusID = 5;//Запрос на закрытие контракта
                    contract.Bid.BidStatusID = 7;//Контракт выполнен

                    db.Entry(contract).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("BidDetails", "Bid", new { id = contract.BidID });
                }
                else
                    ViewBag.Message = "Выберите пункт \"Я согласен\" и заполните отчёт!";

                return View(contract); 
            }
            ViewBag.Message = "Возникла ошибка с передачей данных!";
            return View(contract);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}