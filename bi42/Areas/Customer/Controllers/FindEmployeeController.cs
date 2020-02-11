using Bi42.Models;
using PagedList;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace bi42.Areas.Customer.Controllers
{
    public class FindEmployeeController : Controller
    {
        private DbModel db = new DbModel();
        int pageSize = 25;


        [HttpGet]
        public ActionResult Index(int? OrderAreaId, string StrSearch, int? page)
        {
            IList<Profile> model = new List<Profile>();
            var selectList = new SelectList(db.OrderAreas, "OrderAreaId", "Name", OrderAreaId).ToList();
            selectList.Add(new SelectListItem { Value = "-1", Text = "Все группы", Selected = OrderAreaId > -1 ? true : false });
            ViewBag.OrderAreas = selectList;
            int pageNumber = page??1;
            OrderAreaId = OrderAreaId ?? -1;
            ViewBag.OrderAreaId = OrderAreaId;
            ViewBag.StrSearch = StrSearch;

            var query = from p in db.Profiles
                        where p.ProfileTypeID == 2 
                            && (OrderAreaId == -1 || p.OrderAreaID == OrderAreaId) 
                            && p.IsActive==true
                        select p;

            if (StrSearch != null && !"".Equals(StrSearch))
                query = query.Where<Profile>(x => x.Name.Contains(StrSearch) || x.Description.Contains(StrSearch));

            query = query.OrderBy(x => x.Name);

            return View(query.ToPagedList<Profile>(pageNumber, pageSize));
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

            if (StrSearch != null && !"".Equals(StrSearch))
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