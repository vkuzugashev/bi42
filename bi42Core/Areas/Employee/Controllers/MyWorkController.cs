using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bi42.Models;
using System.IO;
using System.Web.Security;
using System.Drawing;
using System.Configuration;


namespace bi42.Areas.Employer.Controllers
{
    public class MyWorkController : Controller
    {
        private DbModel db = new DbModel();

        //
        // GET: /Employer/MyWork
        [Authorize]
        public ActionResult Index()
        {
            string UserID = User.Identity.Name;
            Profile profile = db.Profiles.SingleOrDefault(p => p.UserID == UserID && p.ProfileTypeID == 2 && p.IsActive == true);
            if (profile == null)
            {
                //Нет профиля перенаправил на вкладку профилей
                return RedirectToAction("Index", "Profile");
            }

            IList<MyWork> myworks = db.MyWorks.Where(x => x.UserID == User.Identity.Name).ToList<MyWork>();
            return View(myworks);
        }

        //
        // GET: /Employer/MyWork/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {
            MyWork mywork = db.MyWorks.SingleOrDefault(x => x.MyWorkID == id && x.UserID == User.Identity.Name);
            if (mywork == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfileID = new SelectList(db.Profiles.Where(x => x.ProfileTypeID == 2 && x.UserID == User.Identity.Name), "ProfileID", "Name");
            return View(mywork);
        }

        //
        // GET: /Employer/MyWorkCreate

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ProfileID = new SelectList(db.Profiles.Where(x => x.ProfileTypeID == 2 && x.UserID == User.Identity.Name), "ProfileID", "Name");
            return View();
        }

        //
        // POST: /Employer/MyWork/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(MyWork mywork, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                mywork.UserID = User.Identity.Name;
                db.MyWorks.Add(mywork);
                db.SaveChanges();
                if (file != null && file.ContentLength > 0)
                {
                    string filename = "mywork" + mywork.MyWorkID + ".jpg";
                    
                    string dir = Server.MapPath("~/Content/Profiles/Profile" + mywork.ProfileID + "/MyWorks");
                    if(!System.IO.Directory.Exists(dir))
                        System.IO.Directory.CreateDirectory(dir);

                    var path = Path.Combine(Server.MapPath("~/Content/Profiles/Profile" + mywork.ProfileID + "/MyWorks"), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    mywork.MyWorkImage = "/Content/Profiles/Profile"+mywork.ProfileID+"/MyWorks/" + filename;
                    //изменим размер картинки
                    Image image = SharedLib.ResizeImg(Image.FromStream(file.InputStream), int.Parse(ConfigurationManager.AppSettings["mywork.width"]), 0);
                    image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfileID = new SelectList(db.Profiles.Where(x => x.ProfileTypeID == 2 && x.UserID == User.Identity.Name), "ProfileID", "Name");
            return View(mywork);
        }

        //
        // GET: /Employer/MyWork/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            MyWork mywork = db.MyWorks.SingleOrDefault(x => x.MyWorkID==id && x.UserID == User.Identity.Name);
            if (mywork == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfileID = new SelectList(db.Profiles.Where(x => x.ProfileTypeID == 2 && x.UserID == User.Identity.Name), "ProfileID", "Name");
            return View(mywork);
        }

        //
        // POST: /Employer/MyWorkEdit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, bool? delAttach, HttpPostedFileBase file)
        {
            MyWork mywork = db.MyWorks.SingleOrDefault(x => x.MyWorkID == id && x.UserID == User.Identity.Name);
            if (TryUpdateModel<MyWork>(mywork))
            {
                if (delAttach==true)
                {
                    string filename = "mywork" + mywork.MyWorkID + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Content/Profiles/Profile" + mywork.ProfileID + "/MyWorks"), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    mywork.MyWorkImage = null;
                }
                if (file != null && file.ContentLength > 0)
                {
                    string filename = "mywork" + mywork.MyWorkID + ".jpg";
                    string dir = Server.MapPath("~/Content/Profiles/Profile" + mywork.ProfileID + "/MyWorks");
                    if (!System.IO.Directory.Exists(dir))
                        System.IO.Directory.CreateDirectory(dir);
                    var path = Path.Combine(Server.MapPath("~/Content/Profiles/Profile" + mywork.ProfileID + "/MyWorks"), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    mywork.MyWorkImage = "/Content/Profiles/Profile" + mywork.ProfileID + "/MyWorks/" + filename;
                    //изменим размер картинки
                    Image image = SharedLib.ResizeImg(Image.FromStream(file.InputStream), int.Parse(ConfigurationManager.AppSettings["mywork.width"]), 0);
                    image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                mywork.UserID = User.Identity.Name;                
                db.Entry(mywork).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfileID = new SelectList(db.Profiles.Where(x => x.ProfileTypeID == 2 && x.UserID == User.Identity.Name), "ProfileID", "Name");
            return View(mywork);
        }

        //
        // GET: /Employer/Profile/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            MyWork mywork = db.MyWorks.SingleOrDefault(x => x.MyWorkID == id && x.UserID == User.Identity.Name);
            if (mywork == null)
            {
                return HttpNotFound();
            }
            return View(mywork);
        }

        //
        // POST: /Employer/Profile/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            MyWork mywork = db.MyWorks.SingleOrDefault(x => x.MyWorkID == id && x.UserID == User.Identity.Name);
            if (mywork == null)
            {
                return HttpNotFound();
            }
            db.MyWorks.Remove(mywork);
            db.SaveChanges();
            //Удалим картинку товара
            string filename = "mywork" + mywork.MyWorkID + ".jpg";
            var path = Path.Combine(Server.MapPath("~/Content/Profiles/Profile" + mywork.ProfileID + "/MyWorks"), filename);
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