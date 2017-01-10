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


namespace bi42.Areas.MyShop.Controllers
{
    public class ShopBoardsController : Controller
    {
        private DbModel db = new DbModel();

        //
        // GET: /Shop/Goods/
        [Authorize]
        public ActionResult Index()
        {
            IList<ShopBoard> shopboards = db.ShopBoards.Include("Shop").Where(x => x.Shop.UserID == User.Identity.Name).ToList<ShopBoard>();
            if (db.Shops.Where(x => x.UserID == User.Identity.Name).Count() > 0)
                ViewBag.IsExistShop = true;
            else
                ViewBag.IsExistShop = false;
            return View(shopboards);
        }

        //
        // GET: /Goods/Commodity/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {
            ShopBoard shopboard = db.ShopBoards.Include("Shop").SingleOrDefault(x => x.ShopBoardID == id && x.Shop.UserID == User.Identity.Name);
            if (shopboard == null)
            {
                return HttpNotFound();
            }
            return View(shopboard);
        }

        //
        // GET: /Goods/Commodity/Create

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Goods/Commodity/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(ShopBoard shopboard, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                shopboard.ShopID = db.Shops.SingleOrDefault(x => x.UserID == User.Identity.Name).ShopID;
                db.ShopBoards.Add(shopboard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shopboard);
        }

        //
        // GET: /Goods/Commodity/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            ShopBoard shopboard = db.ShopBoards.Include("Shop").SingleOrDefault(x => x.ShopBoardID == id && x.Shop.UserID == User.Identity.Name);
            if (shopboard == null)
            {
                return HttpNotFound();
            }            
            return View(shopboard);
        }

        //
        // POST: /Goods/Commodity/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, ShopBoard shopboard)
        {
            Shop shop = db.Shops.SingleOrDefault(x => x.UserID == User.Identity.Name);
            if (shop == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                shopboard.ShopID = shop.ShopID;
                db.Entry(shopboard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shopboard);
        }

        //
        // GET: /Goods/Commodity/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            ShopBoard shopboard = db.ShopBoards.Include("Shop").SingleOrDefault(x => x.ShopBoardID == id && x.Shop.UserID == User.Identity.Name);
            if (shopboard == null)
            {
                return HttpNotFound();
            }
            return View(shopboard);
        }

        //
        // POST: /Goods/Commodity/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            ShopBoard shopboard = db.ShopBoards.Include("Shop").SingleOrDefault(x => x.ShopBoardID == id && x.Shop.UserID == User.Identity.Name);
            db.ShopBoards.Remove(shopboard);
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