using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Bi42.Models;
using System.Web.Security;
using System.Web;
using System.IO;
using PagedList;

namespace bi42.Areas.Employer.Controllers
{
    public class OrderTaskController : Controller
    {
        private DbModel db = new DbModel();
        int pageSize = 10;

        [Authorize]
        public ActionResult Index(int orderid, int? page)
        {
            string UserID = User.Identity.Name;
            Order order = db.Orders.First<Order>(x => x.OrderID == orderid);
            Profile profile = db.Profiles.SingleOrDefault(p => p.UserID == UserID && p.ProfileTypeID == 2 && p.IsActive == true);
            ViewBag.BidID = order.Bids.Where(x=>x.UserID==UserID).First().BidID;
            ViewBag.OrderName = order.Name;
            ViewBag.OrderDescription = order.Description;
            db.SetLastAccess("emp_order_task_" + orderid, UserID);
            IPagedList<OrderTask> tasks = db.OrderTasks
                .Where(x => x.OrderID == orderid && x.ToProfileID==profile.ProfileID)
                .OrderByDescending(x=>x.UpdateTime)
                .ToPagedList<OrderTask>(page??1, pageSize);
            return View(tasks);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult OrderTaskEdit(int orderid, int id)
        {
            OrderTask task;
            if (id != 0)
            {
                task = db.OrderTasks.Find(id);
                if (task == null)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                task = new OrderTask();
                task.OrderID = orderid;
            }

            ViewBag.ToProfileID = new SelectList(db.Orders.Find(task.OrderID).Contracts, "Bid.ProfileID", "Bid.Profile.Name", task.ToProfileID);
            ViewBag.TaskStatusID = new SelectList(db.TaskStatuses, "TaskStatusID", "Name", task.TaskStatusID);
            return View(task);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OrderTaskUpdate(int orderid, int id, bool? delAttach, HttpPostedFileBase file)
        {
            OrderTask task;

            if (id != 0)
            {
                task = db.OrderTasks.Find(id);
                if (task == null)
                {
                    return HttpNotFound();
                }
            }
            else
                task = new OrderTask();

            if (TryUpdateModel<OrderTask>(task))
            {
                if (task.PublishTime == null)
                    task.PublishTime = DateTime.Now;

                //task.UpdateTime = DateTime.Now;

                if (id == 0)
                    db.OrderTasks.Add(task);
                if (task.Order == null)
                    task.Order = db.Orders.Find(orderid);

                string url = "~/Content/Profiles/Profile" + task.Order.ProfileID + "/Orders/" + task.OrderID;
                string dir = Server.MapPath(url);
                string filename = "task" + id;
                SharedLib.UploadFile(dir, filename, delAttach, file);

                if (delAttach == true)
                {
                    task.TaskFile = null;
                    task.ContentType = null;
                    task.FileName = null;
                }
                if (file != null && file.ContentLength > 0)
                {
                    task.TaskFile = url + "/" + filename;
                    task.ContentType = file.ContentType;
                    task.FileName = file.FileName;
                }

                db.SaveChanges();
                return RedirectToAction("Index", "OrderTask", new { orderid = task.OrderID });
            }
            else
            {
                TempData["Message"] = "Необходимо принять заявку исполнителя и заполнить текст контракта!";
                TempData["ContractText"] = Request.Params["ContractText"];
                return RedirectToAction("OrderTaskEdit", new { orderid = task.OrderID, id = task.OrderTaskID });
            }
        }

        [HttpGet]
        public FileStreamResult GetTaskFile(int id)
        {
            OrderTask task;
            task = db.OrderTasks.Find(id);

            if (task == null)
                return null;

            string filePath = Server.MapPath(task.TaskFile);
            if (System.IO.File.Exists(filePath))
            {
                Response.Clear();
                Response.ContentType = task.ContentType;
                Response.AddHeader("content-disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(task.FileName) + "\"");
                return File(new FileStream(filePath, FileMode.Open), task.ContentType);
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