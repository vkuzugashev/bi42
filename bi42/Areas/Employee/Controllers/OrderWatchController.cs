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
    public class OrderWatchController : Controller
    {
        private DbModel db = new DbModel();

        [Authorize]
        public ActionResult Index()
        {
            string UserID = User.Identity.Name;
            Profile profile = db.Profiles.SingleOrDefault(p => p.UserID == UserID && p.ProfileTypeID == 2 && p.IsActive == true);
            if(profile==null)
            {
                //Нет профиля перенаправил на вкладку профилей
                return RedirectToAction("Index","Profile");
            }
            IList<OrderWatch> model = new List<OrderWatch>();
            IList<OrderWatch> orders = db.OrderWatches.Where(x => x.UserID == UserID && x.Order.OrderAreaID == profile.OrderAreaID).ToList<OrderWatch>();
            foreach (OrderWatch order in orders)
            {
                model.Add(order);
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Details(int orderid = 0)
        {
            Order order = db.Orders.SingleOrDefault(p => p.OrderID == orderid);
            if (order == null)
            {
                return HttpNotFound();
            }
            if (order.Bids.Where(x => x.UserID == User.Identity.Name).Count() == 0)
                ViewBag.IsNewBid = true;
            else
                ViewBag.IsNewBid = false;
            return View(order);
        }

        [Authorize]
        public ActionResult OrderWatchAdd(int orderid)
        {
            string UserID = User.Identity.Name;
            Profile profile = db.Profiles.SingleOrDefault(p => p.UserID == UserID && p.ProfileTypeID == 2 && p.IsActive == true);
            Order order = db.Orders.SingleOrDefault(x => x.OrderID == orderid);
            OrderWatch orderWatch = new OrderWatch();
            orderWatch.Order = order;
            orderWatch.Profile = profile;
            orderWatch.PublishTime = DateTime.Now;
            orderWatch.OrderWatchStatusID = 1;  //Просмотр
            orderWatch.UserID = UserID;
            db.OrderWatches.Add(orderWatch);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [Authorize]
        public ActionResult OrderWatchDelete(int orderid, int id)
        {
            string UserID = User.Identity.Name;
            OrderWatch order = db.OrderWatches.SingleOrDefault(p => p.OrderWatchID == id && p.UserID == UserID);
            if (order == null)
            {
                return HttpNotFound();
            }
            db.OrderWatches.Remove(order);
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