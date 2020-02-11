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
    public class PublicMessageController : Controller
    {
        private DbModel db = new DbModel();

        [Authorize]
        public ActionResult Index(int orderid, int id, string retAction)
        {
            string UserID = User.Identity.Name;
            Order order = db.Orders.First<Order>(x => x.OrderID == orderid);
            ViewBag.OrderID = order.OrderID;
            ViewBag.ID = id;
            ViewBag.Message = order.Name;
            ViewBag.RetAction = retAction;
            IList<PublicOrderMessage> messages = db.PublicOrderMessages.Where(x => x.OrderID == orderid).OrderByDescending(x=>x.PublishTime).ToList<PublicOrderMessage>();
            if (messages.Count > 0)
                ViewBag.LastID = messages[0].PublicOrderMessageID;
            else
                ViewBag.LastID = 0;
            return View(messages);
        }

        // POST: /Employer/Order/MessageCreate/5
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int id, string retAction)
        {
            PublicOrderMessage message = new PublicOrderMessage();
            if (TryUpdateModel<PublicOrderMessage>(message))
            {
                Order order = db.Orders.SingleOrDefault(p => p.OrderID == message.OrderID);
                if (order == null)
                {
                    return HttpNotFound();
                }

                string UserID = User.Identity.Name;
                Profile profile = db.Profiles.SingleOrDefault(p => p.UserID == UserID && p.ProfileTypeID == 2 && p.IsActive == true);
                message.ProfileID = profile.ProfileID;
                message.PublishTime = DateTime.Now;
                message.UserID = UserID;
                db.PublicOrderMessages.Add(message);
                //Удалим проект из избранных
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { orderid=message.OrderID, id = id, retAction = retAction });
        }

        // GET: /Employer/Order/PrivateMessages
        [Authorize]
        public ActionResult GetLastPublicMessages(int orderid, int lastid)
        {
            string UserID = User.Identity.Name;
            Order order = db.Orders.First<Order>(x => x.OrderID == orderid);
            if (order == null)
            {
                return HttpNotFound();
            }
            db.SetLastAccess("emp_pub_msg_" + orderid, UserID);

            IList<PublicMessage> model = new List<PublicMessage>();
            foreach (PublicOrderMessage row in db.PublicOrderMessages.Where(x => x.PublicOrderMessageID > lastid && x.OrderID == order.OrderID).OrderByDescending(x => x.PublishTime).ToList<PublicOrderMessage>())
                model.Add(new PublicMessage()
                {
                    id = row.PublicOrderMessageID,
                    publishtime = ((DateTime)row.PublishTime).ToString("dd.MM.yy HH:mm:ss"),
                    profile = row.Profile.Name,
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