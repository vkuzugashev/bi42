﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bi42.Models;


namespace bi42.Areas.Admin.Controllers
{
    public class ProjectAreaController : Controller
    {
        private DbModel db = new DbModel();

        //
        // GET: /Admin/ProjectArea/

        public ActionResult Index()
        {
            return View(db.OrderAreas.ToList());
        }

        //
        // GET: /Admin/ProjectArea/Details/5

        public ActionResult Details(int id = 0)
        {
            OrderArea projectarea = db.OrderAreas.Single(p => p.OrderAreaID == id);
            if (projectarea == null)
            {
                return HttpNotFound();
            }
            return View(projectarea);
        }

        //
        // GET: /Admin/ProjectArea/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/ProjectArea/Create

        [HttpPost]
        public ActionResult Create(OrderArea projectarea)
        {
            if (ModelState.IsValid)
            {
                db.OrderAreas.Add(projectarea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectarea);
        }

        //
        // GET: /Admin/ProjectArea/Edit/5

        public ActionResult Edit(int id = 0)
        {
            OrderArea projectarea = db.OrderAreas.Single(p => p.OrderAreaID == id);
            if (projectarea == null)
            {
                return HttpNotFound();
            }
            return View(projectarea);
        }

        //
        // POST: /Admin/ProjectArea/Edit/5

        [HttpPost]
        public ActionResult Edit(OrderArea projectarea)
        {
            if (ModelState.IsValid)
            {
                db.OrderAreas.Attach(projectarea);
                db.Entry(projectarea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectarea);
        }

        //
        // GET: /Admin/ProjectArea/Delete/5

        public ActionResult Delete(int id = 0)
        {
            OrderArea projectarea = db.OrderAreas.Single(p => p.OrderAreaID == id);
            if (projectarea == null)
            {
                return HttpNotFound();
            }
            return View(projectarea);
        }

        //
        // POST: /Admin/ProjectArea/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderArea projectarea = db.OrderAreas.Single(p => p.OrderAreaID == id);
            db.OrderAreas.Remove(projectarea);
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