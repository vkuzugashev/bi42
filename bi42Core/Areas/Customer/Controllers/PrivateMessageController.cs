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
    public class PrivateMessageController : Controller
    {
        private DbModel db = new DbModel();


        
        [Authorize]
        public ActionResult Index(int orderid, int bid, string retAction)
        {
            string UserID = User.Identity.Name;
            Order order = db.Orders.First<Order>(x => x.OrderID == orderid);
            ViewBag.OrderID = order.OrderID;
            ViewBag.Message = order.Name;
            ViewBag.BidID = bid;
            ViewBag.RetAction = retAction;  //адрес контролера для возврата
            IList<PrivateOrderMessage> messages = db.PrivateOrderMessages.Where(x => x.OrderID == orderid).OrderByDescending(x => x.PublishTime).ToList<PrivateOrderMessage>();
            if (messages.Count > 0)
                ViewBag.LastID = messages[0].PrivateOrderMessageID;
            else
                ViewBag.LastID = 0;
            return View(messages);
        }

     
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int bid, string retAction)
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
                Profile profile = db.Profiles.SingleOrDefault(p => p.UserID == UserID && p.ProfileTypeID == 1 && p.IsActive == true);
                message.FromProfileID = profile.ProfileID;
                //message.ToProfileID = profile.ProfileID;
                message.PublishTime = DateTime.Now;
                message.UserID = UserID;
                db.PrivateOrderMessages.Add(message);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { id = message.OrderID, bid=bid, retAction=retAction });
        }

        [Authorize]
        public ActionResult GetLastPrivateMessages(int orderid, int lastid)
        {
            string UserID = User.Identity.Name;
            Order order = db.Orders.First<Order>(x => x.OrderID == orderid);
            if (order == null)
            {
                return HttpNotFound();
            }
            db.SetLastAccess("cstm_prv_msg_" + orderid, UserID);

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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}