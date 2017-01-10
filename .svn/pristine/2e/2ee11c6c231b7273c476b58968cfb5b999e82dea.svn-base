using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Bi42.Models;
using System.Web.Security;
using System.Web;
using PagedList;

namespace bi42.Areas.Employer.Controllers
{
    public class FindOrderController : Controller
    {
        private DbModel db = new DbModel();
        int pageSize = 10;

        public ActionResult Index(int? OrderID)
        {
            //string UserID = User.Identity.Name;
            //Profile profile = db.Profiles.SingleOrDefault(p => p.UserID == UserID && p.ProfileTypeID == 2 && p.IsActive == true);
            //if (profile == null)
            //{
            //    //Нет профиля перенаправил на вкладку профилей
            //    return RedirectToAction("Index", "Profile");
            //}
            if (User.Identity.IsAuthenticated)
            {
                string UserID = User.Identity.Name;
                ViewBag.ProfileOrderAreaID = (from p in db.Profiles
                                              where p.UserID == UserID && p.IsActive == true && p.ProfileTypeID == 2
                                              select p.OrderAreaID).FirstOrDefault();
            }
            ViewBag.OrderAreaID = -1;
            var selectList = new SelectList(db.OrderAreas, "OrderAreaId", "Name").ToList();
            selectList.Add(new SelectListItem { Value="-1", Text="Все группы", Selected=true});
            ViewBag.OrderAreas = selectList;
            var query = from o in db.Orders
                        where o.OrderStatusID == 2 && (OrderID==null || o.OrderID==OrderID)
                        orderby o.PublishTime descending
                        select o;

            int pageNumber = 1;
            return View(query.ToPagedList<Order>(pageNumber, pageSize));
        }

        [HttpPost, ActionName("Index")]
        public ActionResult FindOrder(int OrderAreaID, string StrSearch, int? page)
        {
            var selectList = new SelectList(db.OrderAreas, "OrderAreaId", "Name").ToList();
            selectList.Add(new SelectListItem { Value = "-1", Text = "Все группы", Selected = true });
            ViewBag.OrderAreas = selectList;
            ViewBag.OrderAreaID = OrderAreaID;
            ViewBag.StrSearch = StrSearch;
            if (User.Identity.IsAuthenticated)
            {
                string UserID = User.Identity.Name;
                ViewBag.ProfileOrderAreaID = (from p in db.Profiles
                                              where p.UserID == UserID && p.IsActive == true && p.ProfileTypeID == 2
                                              select p.OrderAreaID).FirstOrDefault();
            }
            var query = from o in db.Orders
                        where (OrderAreaID==-1 || o.OrderAreaID == OrderAreaID) && o.OrderStatusID == 2
                        select o;

            if (StrSearch != null || !"".Equals(StrSearch))
                query = query.Where<Order>(x => x.Name.Contains(StrSearch) || x.Description.Contains(StrSearch));

            query = query.OrderByDescending(x => x.PublishTime);

            int pageNumber = (page ?? 1);
            return View(query.ToPagedList<Order>(pageNumber, pageSize));
        }

        [Authorize]
        public ActionResult Details(int orderid = 0)
        {
            Order order = db.Orders.SingleOrDefault(p => p.OrderID == orderid);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}