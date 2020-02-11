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
    public class FeedBackController : Controller
    {
        private DbModel db = new DbModel();

        [Authorize]
        [HttpGet]
        public ActionResult Index(int o, int p, int b)
        {
            string UserID = User.Identity.Name;
            FeedBack feedBack = db.FeedBacks.FirstOrDefault(x => x.FromProfileID == p && x.OrderID == o && x.UserID == UserID);
            if (feedBack == null)
            {
                feedBack = new FeedBack();
                feedBack.UserID = UserID;
                feedBack.Order = db.Orders.FirstOrDefault<Order>(x => x.OrderID == o);
                feedBack.OrderID = o;
                feedBack.Profile = db.Profiles.FirstOrDefault<Profile>(x => x.ProfileID == feedBack.Order.ProfileID);
                feedBack.ProfileID = feedBack.Order.ProfileID;
                feedBack.FromProfile = db.Profiles.FirstOrDefault<Profile>(x => x.ProfileID == p);
                feedBack.FromProfileID = p;
            }
            ViewBag.BidID = b;
            return View(feedBack);
        }

        [Authorize]
        [HttpPost]
        [ActionName("Index")]
        public ActionResult FeedBack(int BidID)
        {
            string UserID = User.Identity.Name;
            FeedBack feedBack = new FeedBack();
            if (TryUpdateModel<FeedBack>(feedBack))
            {
                //Кто даёт отзыв?
                Profile profile = db.Profiles.FirstOrDefault(x => x.UserID == UserID && x.ProfileTypeID == 2 && x.IsActive);
                feedBack.UserID = UserID;
                feedBack.FromProfileID = profile.ProfileID;
                feedBack.PublishTime = DateTime.Now;
                if (feedBack.FeedBackID == 0)
                    db.FeedBacks.Add(feedBack);
                else
                    db.Entry<FeedBack>(feedBack).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("BidDetails", "Bid", new { id = BidID });
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}