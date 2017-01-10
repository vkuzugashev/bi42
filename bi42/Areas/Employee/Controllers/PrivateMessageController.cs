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
    public class PrivateMessageController : Controller
    {
        private DbModel db = new DbModel();

        // GET: /Employer/Order/PrivateMessages
        [Authorize]
        public ActionResult Index(int id, string retAction)
        {
            string UserID = User.Identity.Name;
            Bid bid = db.Bids.First<Bid>(x => x.BidID == id);
            ViewBag.OrderID = bid.OrderID;
            ViewBag.Message = bid.OrderName;
            ViewBag.BidID = bid.BidID;
            ViewBag.RetAction = retAction;
            IList<PrivateOrderMessage> messages = db.PrivateOrderMessages.Where(x => x.OrderID == bid.OrderID).OrderByDescending(x => x.PublishTime).ToList<PrivateOrderMessage>();
            if (messages.Count > 0)
                ViewBag.LastID = messages[0].PrivateOrderMessageID;
            else
                ViewBag.LastID = 0;
            return View(messages);
        }

        [Authorize]
        public ActionResult GetLastPrivateMessages(int id, int lastid)
        {
            string UserID = User.Identity.Name;
            Order order = db.Orders.First<Order>(x => x.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            db.SetLastAccess("emp_prv_msg_" + id, UserID);
            IList<PrivateMessage> model = new List<PrivateMessage>();
            foreach (PrivateOrderMessage row in db.PrivateOrderMessages.Where(x => x.PrivateOrderMessageID > lastid && x.OrderID == order.OrderID).OrderByDescending(x => x.PublishTime).ToList<PrivateOrderMessage>())
                model.Add(new PrivateMessage()
                {
                    id = row.PrivateOrderMessageID,
                    publishtime = ((DateTime)row.PublishTime).ToString("dd.MM.yy HH:mm:ss"),
                    profile = row.FromProfile.Name,
                    message = row.Message
                });
            return new JsonResult { Data = new { rows = model, rowcount = model.Count }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        // POST: /Employer/Order/MessageCreate/5
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int id, string retAction)
        {
            PrivateOrderMessage message = new PrivateOrderMessage();
            if (TryUpdateModel<PrivateOrderMessage>(message))
            {
                Order order = db.Orders.SingleOrDefault(p => p.OrderID == message.OrderID);
                if (order == null)
                {
                    return HttpNotFound();
                }

                string UserID = User.Identity.Name;
                Profile profile = db.Profiles.SingleOrDefault(p => p.UserID == UserID && p.ProfileTypeID == 2 && p.IsActive == true);
                message.FromProfileID = profile.ProfileID;
                //message.ToProfileID = profile.ProfileID;
                message.PublishTime = DateTime.Now;
                message.UserID = UserID;
                db.PrivateOrderMessages.Add(message);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { id = id, retAction=retAction });
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}