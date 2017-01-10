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
    public class ShopConfigController : Controller
    {
        private DbModel db = new DbModel();

        //
        // GET: /Shop/Profile/
        [Authorize]
        public ActionResult Index()
        {
            Bi42.Models.Shop shop = db.Shops.SingleOrDefault(x => x.UserID == User.Identity.Name);
            return View(shop);
        }

        //
        // GET: /Shop/Shop/Create

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ShopAreaID = new SelectList(db.ShopAreas, "ShopAreaID", "Name");
            return View();
        }

        //
        // POST: /Shop/Shop/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(Bi42.Models.Shop shop, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                shop.UserID = User.Identity.Name;
                if (shop.IsActive == true)
                    shop.PublishTime = DateTime.Now;
                db.Shops.Add(shop);
                db.SaveChanges();
                if (file != null && file.ContentLength > 0)
                {
                    string shopdir = Server.MapPath("~/Content/Shops/")+"Shop"+shop.ShopID;
                    if (!System.IO.Directory.Exists(shopdir))
                        System.IO.Directory.CreateDirectory(shopdir);

                    string filename = "logo" + shop.ShopID + ".jpg";
                    var path = Path.Combine(shopdir, filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    shop.LogoImage = "/Content/Shops/Shop"+shop.ShopID+"/" + filename;
                    //изменим размер картинки
                    Image image = SharedLib.ResizeImg(Image.FromStream(file.InputStream), int.Parse(ConfigurationManager.AppSettings["myshop.logo.width"]), 0);
                    image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShopAreaID = new SelectList(db.ShopAreas, "ShopAreaID", "Name", shop.ShopArea.ShopAreaID);

            return View(shop);
        }

        //
        // GET: /Shop/Shop/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Bi42.Models.Shop shop = db.Shops.SingleOrDefault(x => x.ShopID == id && x.UserID == User.Identity.Name);
            if (shop == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShopAreaID = new SelectList(db.ShopAreas, "ShopAreaID", "Name", shop.ShopArea.ShopAreaID);
            
            return View(shop);
        }

        //
        // POST: /Shop/Shop/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, bool? delAttach, HttpPostedFileBase file)
        {
            Bi42.Models.Shop shop = db.Shops.SingleOrDefault(x => x.ShopID == id && x.UserID == User.Identity.Name);
            if (shop == null)
            {
                return HttpNotFound();
            }
            if (TryUpdateModel<Bi42.Models.Shop>(shop))
            {
                if (delAttach==true)
                {

                    string filename = "logo" + shop.ShopID + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Content/Shops/Shop"+shop.ShopID), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    shop.LogoImage = null;
                }
                if (file != null && file.ContentLength > 0)
                {
                    string filename = "logo" + shop.ShopID + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Content/Shops/Shop" + shop.ShopID), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    shop.LogoImage = "/Content/Shops/Shop" + shop.ShopID + "/" + filename;
                    //изменим размер картинки
                    Image image = SharedLib.ResizeImg(Image.FromStream(file.InputStream), int.Parse(ConfigurationManager.AppSettings["myshop.logo.width"]), 0);
                    image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                shop.UserID = User.Identity.Name;
                if (shop.PublishTime == null && shop.IsActive == true)
                    shop.PublishTime = DateTime.Now;
                else if (shop.PublishTime != null && shop.IsActive == false)
                    shop.PublishTime = null;
                db.Entry(shop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShopAreaID = new SelectList(db.ShopAreas, "ShopAreaID", "Name", shop.ShopArea.ShopAreaID);
            return View(shop);
        }

        // GET: /Shop/Shop/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Bi42.Models.Shop shop = db.Shops.SingleOrDefault(x => x.ShopID == id && x.UserID == User.Identity.Name);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        //
        // POST: /Shop/Shop/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Bi42.Models.Shop shop = db.Shops.SingleOrDefault(x => x.ShopID == id && x.UserID == User.Identity.Name);
            if (shop == null)
            {
                return HttpNotFound();
            }
            db.Shops.Remove(shop);
            db.SaveChanges();
            //Удалим каталог магазина рекурсивно
            System.IO.Directory.Delete(Server.MapPath("~/Content/Shops/Shop" + shop.ShopID), true);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}