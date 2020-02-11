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
    public class FeedBackController : Controller
    {
        private DbModel db = new DbModel();

        [Authorize]
        [HttpGet]
        public ActionResult Index(int p, int o)
        {
            string UserID = User.Identity.Name;
            FeedBack feedBack = db.FeedBacks.FirstOrDefault(x => x.ProfileID == p && x.OrderID == o && x.UserID == UserID);
            if (feedBack == null)
            {
                feedBack = new FeedBack();
                feedBack.UserID = UserID;
                feedBack.Order = db.Orders.FirstOrDefault<Order>(x => x.OrderID == o);
                feedBack.OrderID = o;
                feedBack.Profile = db.Profiles.FirstOrDefault<Profile>(x => x.ProfileID == p);
                feedBack.ProfileID = p;
                feedBack.FromProfile = db.Profiles.FirstOrDefault<Profile>(x => x.ProfileID == feedBack.Order.ProfileID);
                feedBack.FromProfileID = feedBack.Order.ProfileID;
            }
            return View(feedBack);
        }

        [Authorize]
        [HttpPost]
        [ActionName("Index")]
        public ActionResult FeedBack()
        {
            string UserID = User.Identity.Name;
            FeedBack feedBack = new FeedBack();
            if (TryUpdateModel<FeedBack>(feedBack))
            {
                //Кто даёт отзыв?
                Profile profile = db.Profiles.FirstOrDefault(x => x.UserID == UserID && x.ProfileTypeID == 1 && x.IsActive);
                feedBack.UserID = UserID;
                feedBack.FromProfileID = profile.ProfileID;
                feedBack.PublishTime = DateTime.Now;
                if (feedBack.FeedBackID == 0)
                    db.FeedBacks.Add(feedBack);
                else
                    db.Entry<FeedBack>(feedBack).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Order", new { orderid = feedBack.OrderID });
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}