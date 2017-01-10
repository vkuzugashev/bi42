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
using PagedList;

namespace bi42.Areas.Customer.Controllers
{
    public class FindEmployeeController : Controller
    {
        private DbModel db = new DbModel();
        int pageSize = 10;


        [HttpGet]
        public ActionResult Index()
        {
            IList<Profile> model = new List<Profile>();
            var selectList = new SelectList(db.OrderAreas, "OrderAreaId", "Name").ToList();
            selectList.Add(new SelectListItem { Value = "-1", Text = "Все группы", Selected = true });
            ViewBag.OrderAreas = selectList;
            //int pageNumber = 1;
            //return View(model.ToPagedList<Profile>(pageNumber, pageSize));
            var query = from p in db.Profiles
                        where p.ProfileTypeID == 2 && p.IsActive==true
                        orderby p.PublishTime descending
                        select p;

            return View(query.ToPagedList<Profile>(1, pageSize));
        }

        [HttpPost, ActionName("Index")]
        /// <summary>
        /// Найти исполнителя
        /// </summary>
        /// <returns></returns>        
        public ActionResult Find(int OrderAreaId, string StrSearch, int? page)
        {
            var selectList = new SelectList(db.OrderAreas, "OrderAreaId", "Name").ToList();
            selectList.Add(new SelectListItem { Value = "-1", Text = "Все группы", Selected = true });
            ViewBag.OrderAreas = selectList;
            //ViewBag.OrderAreas = new SelectList(db.OrderAreas, "OrderAreaId", "Name");
            ViewBag.OrderAreaId = OrderAreaId;
            ViewBag.StrSearch = StrSearch;
            string UserID = User.Identity.Name;

            var query = from p in db.Profiles
                        where (OrderAreaId==-1 || p.OrderAreaID == OrderAreaId) && p.ProfileTypeID == 2 && p.IsActive == true
                        select p;

            if (StrSearch != null || !"".Equals(StrSearch))
                query = query.Where<Profile>(x => x.Name.Contains(StrSearch) || x.Description.Contains(StrSearch));

            query = query.OrderBy(x=>x.Name);

            return View(query.ToPagedList<Profile>(page??1, pageSize));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}