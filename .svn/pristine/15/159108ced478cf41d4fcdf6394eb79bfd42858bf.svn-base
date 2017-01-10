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
using System.Drawing;
using System.Configuration;


namespace bi42.Areas.MyShop.Controllers
{
    public class GoodsController : Controller
    {
        private DbModel db = new DbModel();

        //
        // GET: /Shop/Goods/
        [Authorize]
        public ActionResult Index()
        {
            IList<Commodity> goods = db.Commodities.Include("Shop").Where(x => x.Shop.UserID == User.Identity.Name).ToList<Commodity>();
            if(db.Shops.Where(x=>x.UserID==User.Identity.Name).Count() > 0)
                ViewBag.IsExistShop = true;
            else
                ViewBag.IsExistShop = false;
            return View(goods);
        }

        //
        // GET: /Goods/Commodity/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Commodity commodity = db.Commodities.Include("Shop").SingleOrDefault(x => x.CommodityID == id && x.Shop.UserID == User.Identity.Name);
            if (commodity == null)
            {
                return HttpNotFound();
            }
            return View(commodity);
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
        public ActionResult Create(Commodity commodity, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                commodity.ShopID = db.Shops.SingleOrDefault(x => x.UserID == User.Identity.Name).ShopID;
                db.Commodities.Add(commodity);
                db.SaveChanges();
                if (file != null && file.ContentLength > 0)
                {
                    string filename = "photo" + commodity.CommodityID + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Content/Shops/Shop" + commodity.ShopID), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    commodity.PhotoImage = "/Content/Shops/Shop" + commodity.ShopID +"/"+ filename;
                    //изменим размер картинки
                    Image image = SharedLib.ResizeImg(Image.FromStream(file.InputStream), int.Parse(ConfigurationManager.AppSettings["goods.photo.width"]), 0);
                    image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(commodity);
        }

        //
        // GET: /Goods/Commodity/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Commodity commodity = db.Commodities.Include("Shop").SingleOrDefault(x => x.CommodityID == id && x.Shop.UserID == User.Identity.Name);
            if (commodity == null)
            {
                return HttpNotFound();
            }            
            return View(commodity);
        }

        //
        // POST: /Goods/Commodity/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, bool? delAttach, HttpPostedFileBase file)
        {
            Commodity commodity = db.Commodities.Include("Shop").SingleOrDefault(x => x.CommodityID == id && x.Shop.UserID == User.Identity.Name);
            if (TryUpdateModel<Commodity>(commodity))
            {
                if (delAttach==true)
                {
                    string filename = "logo" + commodity.CommodityID + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Content/Shops/Shop" + commodity.ShopID), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    commodity.PhotoImage = null;
                }
                if (file != null && file.ContentLength > 0)
                {
                    string filename = "photo" + commodity.CommodityID + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Content/Shops/Shop" + commodity.ShopID), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    commodity.PhotoImage = "/Content/Shops/Shop" + commodity.ShopID + "/" + filename;
                    //изменим размер картинки
                    Image image = SharedLib.ResizeImg(Image.FromStream(file.InputStream), int.Parse(ConfigurationManager.AppSettings["goods.photo.width"]), 0);
                    image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                }
                db.Entry(commodity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(commodity);
        }

        //
        // GET: /Goods/Commodity/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Commodity commodity = db.Commodities.Include("Shop").SingleOrDefault(x => x.CommodityID == id && x.Shop.UserID == User.Identity.Name);
            if (commodity == null)
            {
                return HttpNotFound();
            }
            return View(commodity);
        }

        //
        // POST: /Goods/Commodity/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Commodity commodity = db.Commodities.Include("Shop").SingleOrDefault(x => x.CommodityID == id && x.Shop.UserID == User.Identity.Name);
            db.Commodities.Remove(commodity);
            db.SaveChanges();
            //Удалим картинку товара
            string filename = "photo" + commodity.CommodityID + ".jpg";
            var path = Path.Combine(Server.MapPath("~/Content/Shops/Shop" + commodity.ShopID), filename);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}