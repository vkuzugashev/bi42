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


namespace bi42.Areas.Customer.Controllers
{
    public class ProfileController : Controller
    {
        private DbModel db = new DbModel();

        //
        // GET: /Customer/Profile/
        [Authorize]
        public ActionResult Index()
        {
            string UserID = User.Identity.Name;
            IList<Profile> profiles = db.Profiles.Include("OrderArea").Where(x => x.ProfileTypeID == 1 && x.UserID == UserID).ToList<Profile>();
            return View(profiles);
        }

        //
        // GET: /Customer/Profile/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Profile profile = db.Profiles.Single(p => p.ProfileID == id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        //
        // GET: /Customer/Profile/Create

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.OrderAreaID = new SelectList(db.OrderAreas, "OrderAreaID", "Name");
            return View();
        }

        //
        // POST: /Customer/Profile/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(Profile profile, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                profile.ProfileTypeID = 1;//Customer
                profile.UserID = User.Identity.Name;
                if (profile.IsActive == true)
                    profile.PublishTime = DateTime.Now;
                db.Profiles.Add(profile);
                db.SaveChanges();
                if (file != null && file.ContentLength > 0)
                {
                    
                    string dir = Server.MapPath("~/Content/Profiles/Profile")+profile.ProfileID;
                    if (!System.IO.Directory.Exists(dir))
                        System.IO.Directory.CreateDirectory(dir);

                    string filename = "logo" + profile.ProfileID + ".jpg";

                    var path = Path.Combine(dir, filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    profile.LogoImage = "/Content/Profiles/Profile" + profile.ProfileID + "/" + filename;
                    //изменим размер картинки
                    Image image = SharedLib.ResizeImg(Image.FromStream(file.InputStream), int.Parse(ConfigurationManager.AppSettings["profile.logo.width"]), 0);
                    image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                }
                db.SaveChanges();
                if(profile.IsActive==true)
                    CheckActive(profile.ProfileID);
                return RedirectToAction("Index");
            }

            ViewBag.OrderAreaID = new SelectList(db.OrderAreas, "OrderAreaID", "Name", profile.OrderArea.OrderAreaID);

            return View(profile);
        }

        //
        // GET: /Customer/Profile/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Profile profile = db.Profiles.Single(p => p.ProfileID == id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderAreaID = new SelectList(db.OrderAreas, "OrderAreaID", "Name", profile.OrderArea.OrderAreaID);
            
            return View(profile);
        }

        //
        // POST: /Customer/Profile/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, bool? delAttach, HttpPostedFileBase file)
        {
            Profile profile = db.Profiles.Single(p => p.ProfileID == id);
            if (TryUpdateModel<Profile>(profile))
            {
                string dir = Server.MapPath("~/Content/Profiles/Profile") + profile.ProfileID;
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                if (delAttach == true)
                {
                    string filename = "logo" + profile.ProfileID + ".jpg";
                    var path = Path.Combine(dir, filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    profile.LogoImage = null;
                }
                if (file != null && file.ContentLength > 0)
                {
                    string filename = "logo" + profile.ProfileID + ".jpg";
                    var path = Path.Combine(dir, filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    profile.LogoImage = "/Content/Profiles/Profile" + profile.ProfileID + "/" + filename;
                    //изменим размер картинки
                    Image image = SharedLib.ResizeImg(Image.FromStream(file.InputStream), int.Parse(ConfigurationManager.AppSettings["profile.logo.width"]), 0);
                    image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                profile.ProfileTypeID = 1;
                profile.UserID = User.Identity.Name;
                if (profile.PublishTime == null && profile.IsActive == true)
                    profile.PublishTime = DateTime.Now;
                else if (profile.PublishTime != null && profile.IsActive == false)
                    profile.PublishTime = null;
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                if(profile.IsActive==true)
                    CheckActive(profile.ProfileID);
                return RedirectToAction("Index");
            }
            ViewBag.OrderAreaID = new SelectList(db.OrderAreas, "OrderAreaID", "Name", profile.OrderArea.OrderAreaID);
            return View(profile);
        }

        /// <summary>
        /// Прверить что бы профиль был всегда один активен
        /// </summary>
        /// <param name="id"></param>
        private void CheckActive(int ActiveProfileID)
        {
            string UserID = User.Identity.Name;
            IList<Profile> profiles = db.Profiles.Where(p => p.ProfileTypeID==1 && p.ProfileID != ActiveProfileID && p.IsActive == true && p.UserID == UserID).ToList<Profile>();
            if (profiles.Count > 0)
            {
                foreach (Profile profile in profiles)
                {
                    profile.IsActive = false;
                    profile.PublishTime = null;
                    db.Entry(profile).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }
        //
        // GET: /Customer/Profile/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            string UserID = User.Identity.Name;
            Profile profile = db.Profiles.Single(p => p.ProfileID == id && p.UserID == UserID && p.ProfileTypeID==1);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        //
        // POST: /Customer/Profile/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            string UserID = User.Identity.Name;
            Profile profile = db.Profiles.Single(p => p.ProfileID == id && p.UserID == UserID && p.ProfileTypeID == 1); 
            db.Profiles.Remove(profile);
            db.SaveChanges();
            //Удалим профиль
            string dir = Server.MapPath("~/Content/Profiles/Profile" + profile.ProfileID);
            if (System.IO.Directory.Exists(dir))
                System.IO.Directory.Delete(dir, true);
            return RedirectToAction("Index");
        }

        //
        // GET: /Employer/Profile/View/5
        public ActionResult ProfileView(int id = 0)
        {
            Profile profile = db.Profiles.FirstOrDefault(p => p.ProfileID == id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}