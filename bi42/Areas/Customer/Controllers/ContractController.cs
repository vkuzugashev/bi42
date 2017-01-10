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
            return View(contract);
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


        

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}