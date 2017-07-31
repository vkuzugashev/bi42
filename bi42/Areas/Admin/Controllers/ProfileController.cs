using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bi42.Models;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Collections;
using PagedList;

namespace bi42.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProfileController : Controller
    {
        private DbModel db = new DbModel();
        int pageSize = 10;

        public ActionResult Index(int? OrderAreaId, string StrSearch, int? page)
        {
            IList<Profile> model = new List<Profile>();
            var selectList = new SelectList(db.OrderAreas, "OrderAreaId", "Name", OrderAreaId).ToList();
            selectList.Add(new SelectListItem { Value = "-1", Text = "Все группы", Selected = OrderAreaId > -1 ? true : false });
            ViewBag.OrderAreas = selectList;
            int pageNumber = page ?? 1;
            OrderAreaId = OrderAreaId ?? -1;
            ViewBag.OrderAreaId = OrderAreaId;
            ViewBag.StrSearch = StrSearch;

            var query = from p in db.Profiles
                        where (OrderAreaId == -1 || p.OrderAreaID == OrderAreaId)
                                && p.IsActive == true
                        select p;

            if (StrSearch != null && !"".Equals(StrSearch))
                query = query.Where<Profile>(x => x.Name.Contains(StrSearch) || x.Description.Contains(StrSearch));

            query = query.OrderBy(x => x.Name);

            return View(query.ToPagedList<Profile>(pageNumber, pageSize));
        }

        [HttpPost, ActionName("Index")]
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
                        where (OrderAreaId == -1 || p.OrderAreaID == OrderAreaId) && p.IsActive == true
                        select p;

            if (StrSearch != null && !"".Equals(StrSearch))
                query = query.Where<Profile>(x => x.Name.Contains(StrSearch) || x.Description.Contains(StrSearch));

            query = query.OrderBy(x => x.Name);

            return View(query.ToPagedList<Profile>(page ?? 1, pageSize));
        }

        public ActionResult Create(int? OrderAreaId)
        {
            var selectList = new SelectList(db.OrderAreas, "OrderAreaID", "Name", OrderAreaId).ToList();
            selectList.Add(new SelectListItem { Value = "-1", Text = "Все группы"});
            ViewBag.OrderAreaID = selectList;
            var sl = new SelectList(new ArrayList()).ToList();
            sl.Add(new SelectListItem { Text = "Заказчик", Value = "1" });
            sl.Add(new SelectListItem { Text = "Подрядчик", Value = "2" });
            ViewBag.ProfileTypeID = sl;
            return View();
        }

        [HttpGet]
        public ActionResult Details(string StrSearch, int? page, int id = 0)
        {
            Profile profile = db.Profiles.FirstOrDefault(p => p.ProfileID == id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderAreaID = new SelectList(db.OrderAreas, "OrderAreaID", "Name");
            ViewBag.StrSearch = StrSearch;
            ViewBag.page = page;
            return View(profile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Profile profile, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                profile.UserID = profile.Email;
                if (profile.IsActive == true)
                    profile.PublishTime = DateTime.Now;
                db.Profiles.Add(profile);
                db.SaveChanges();
                if (file != null && file.ContentLength > 0)
                {
                    string dir = Server.MapPath("~/Content/Profiles/Profile" + profile.ProfileID);
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
                if (profile.IsActive == true)
                    CheckActive(profile.ProfileID);
                return RedirectToAction("Index", new { OrderAreaId = profile.OrderAreaID });
            }

            ViewBag.OrderAreaID = new SelectList(db.OrderAreas, "OrderAreaID", "Name", profile.OrderArea.OrderAreaID);

            return View(profile);
        }

        [HttpGet]
        public ActionResult Edit(int id, string StrSearch, int? page)
        {
            Profile profile = db.Profiles.FirstOrDefault(p => p.ProfileID == id);
            if (profile == null)
            {
                return HttpNotFound();
            }

            var sl = new SelectList(new ArrayList()).ToList();
            sl.Add(new SelectListItem { Text = "Заказчик", Value = "1", Selected = (profile.ProfileTypeID==1) });
            sl.Add(new SelectListItem { Text = "Подрядчик", Value = "2", Selected = (profile.ProfileTypeID == 2) });
            ViewBag.ProfileTypeID = sl;
            ViewBag.OrderAreaID = new SelectList(db.OrderAreas, "OrderAreaID", "Name", profile.OrderArea.OrderAreaID);
            ViewBag.StrSearch = StrSearch;
            ViewBag.page = page;
            return View(profile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, bool? delAttach, HttpPostedFileBase file, string StrSearch, int? page)
        {
            Profile profile = db.Profiles.FirstOrDefault(p => p.ProfileID == id);
            if (TryUpdateModel<Profile>(profile))
            {
                string dir = Server.MapPath("~/Content/Profiles/Profile" + profile.ProfileID);
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
                profile.UserID = profile.Email;
                if (profile.PublishTime == null && profile.IsActive == true)
                    profile.PublishTime = DateTime.Now;
                else if (profile.PublishTime != null && profile.IsActive == false)
                    profile.PublishTime = null;
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                if (profile.IsActive == true)
                    CheckActive(profile.ProfileID);
                return RedirectToAction("Index", new { OrderAreaId=profile.OrderAreaID, StrSearch=StrSearch, page=page });
            }
            ViewBag.OrderAreaID = new SelectList(db.OrderAreas, "OrderAreaID", "Name", profile.OrderArea.OrderAreaID);
            return View(profile);
        }

        private void CheckActive(int ActiveProfileID)
        {
            Profile prof = db.Profiles.FirstOrDefault(p => p.ProfileID == ActiveProfileID);
            if (prof == null)
                return;
            IList<Profile> profiles = db.Profiles.Where(p => p.ProfileTypeID == 2 && p.ProfileID != ActiveProfileID && p.IsActive == true && p.UserID==prof.UserID).ToList<Profile>();
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

        [HttpGet]
        public ActionResult Delete(int id, string StrSearch, int? page)
        {
            Profile profile = db.Profiles.FirstOrDefault(p => p.ProfileID == id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            ViewBag.StrSearch = StrSearch;
            ViewBag.page = page;
            return View(profile);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id, string StrSearch, int? page)
        {
            Profile profile = db.Profiles.Single(p => p.ProfileID == id);
            db.Profiles.Remove(profile);
            db.SaveChanges();
            //Удалим профиль
            string dir = Server.MapPath("~/Content/Profiles/Profile" + profile.ProfileID);
            if (System.IO.Directory.Exists(dir))
                System.IO.Directory.Delete(dir, true);
            return RedirectToAction("Index", new { OrderAreaId = profile.OrderAreaID, StrSearch = StrSearch, page = page });
        }

        [HttpPost]
        public JsonResult ParseText(string text)
        {
            //->> А 1, веб - студия <<-
            //>> Сферы деятельности:
            //Разработка / поддержка / продвижение web - сайтов, Размещение рекламы в интернете, Дизайн рекламы, Хостинг, Продажа программного обеспечения
            //---------------

            //Орджоникидзе, 50 - 2 офис; 1 этаж
            //тел. + 7 - 923 - 464 - 08 - 18
            //тел. + 7(3843) 60 - 08 - 18
            //http://www.a1st.ru
            //ВКонтакте: http://vk.com/a1st_ru
            //Оплата Наличный расчет, Оплата через Интернет, Оплата через банк
            object data = null;
            text = text.Trim();
            if (text.StartsWith("->>"))
            {
                int startIdx = text.IndexOf("->>") + 3;
                int stopIdx = text.IndexOf("<<-");
                string name = text.Substring(startIdx, stopIdx-startIdx).Trim();
                text = text.Substring(stopIdx+3).Trim();

                string description = "";
                startIdx = text.IndexOf("сти:\n") + 5;
                stopIdx = text.IndexOf("--");
                if (startIdx !=-1 && stopIdx != -1)
                {
                    description = text.Substring(startIdx, stopIdx - startIdx).Trim();
                    text = text.Substring(stopIdx).Trim();
                }

                string addr = "";
                startIdx = text.IndexOf("---------------\n\n");
                if (startIdx != -1)
                {
                    text = text.Substring(startIdx + 17).Trim();
                    stopIdx = text.IndexOf("\n");
                    if (stopIdx != -1)
                    {
                        addr = text.Substring(startIdx, stopIdx - startIdx).Trim();
                        text = text.Substring(stopIdx + 1).Trim();
                    }
                }
                else
                {
                    text = text.Substring(startIdx + 16).Trim();
                }

                string phone = "";
                startIdx = text.IndexOf("тел.");
                if (startIdx != -1)
                {
                    startIdx += 4;
                    stopIdx = text.IndexOf("\n");
                    if (stopIdx != -1)
                        phone = text.Substring(startIdx, stopIdx - startIdx).Trim();
                    else
                        phone = text.Substring(startIdx).Trim();
                    text = text.Substring(stopIdx + 1).Trim();
                }

                startIdx = text.IndexOf("тел.");
                if (startIdx != -1)
                {
                    startIdx += 4;
                    startIdx = text.IndexOf("тел.") + 4;
                    stopIdx = text.IndexOf("\n");
                    if (stopIdx != -1)
                        phone += ", " + text.Substring(startIdx, stopIdx - startIdx).Trim();
                    else
                        phone += ", " + text.Substring(startIdx).Trim();
                    //phone += ", " + text.Substring(startIdx, stopIdx - startIdx).Trim();
                    text = text.Substring(stopIdx + 1).Trim();
                }

                startIdx = text.IndexOf("тел.");
                if (startIdx != -1)
                {
                    startIdx += 4;
                    startIdx = text.IndexOf("тел.") + 4;
                    stopIdx = text.IndexOf("\n");
                    if (stopIdx != -1)
                        phone += ", " + text.Substring(startIdx, stopIdx - startIdx).Trim();
                    else
                        phone += ", " + text.Substring(startIdx).Trim();
                    text = text.Substring(stopIdx + 1).Trim();
                }

                string siteurl = "";
                startIdx = text.IndexOf("http:");
                if (startIdx != -1)
                {
                    stopIdx = text.IndexOf("\n");
                    if(stopIdx != -1)
                        siteurl = text.Substring(startIdx, stopIdx - startIdx).Trim();
                    else
                        siteurl = text.Substring(startIdx).Trim();
                    text = text.Substring(stopIdx + 1).Trim();
                }

                startIdx = text.IndexOf("Оплата");
                if (startIdx != -1)
                {
                    description += "\r\n" + text.Substring(startIdx).Trim();
                }

                data = new { error=false, name=name, addr=addr, phone=phone, siteurl=siteurl, description=description};
            }
            return new JsonResult { Data = data , JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}