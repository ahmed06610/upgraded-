using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using School.Models;

namespace School.Controllers
{
    [Authorize(Roles = RoleName.Manager)]

    public class CourseLevelController : BaseController
    {
        private SchoolEntities1 db = new SchoolEntities1();

        // GET: CourseLevel
        public ActionResult Index()
        {
            return View(db.CourseLevels.ToList());
        }

        // GET: CourseLevel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseLevel courseLevel1 = db.CourseLevels.Find(id);
            if (courseLevel1 == null)
            {
                return HttpNotFound();
            }
            return View(courseLevel1);
        }

        // GET: CourseLevel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseLevel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseLevelId,Level")] CourseLevel courseLevel1)
        {
            if (ModelState.IsValid)
            {
                db.CourseLevels.Add(courseLevel1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(courseLevel1);
        }

        // GET: CourseLevel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseLevel courseLevel1 = db.CourseLevels.Find(id);
            if (courseLevel1 == null)
            {
                return HttpNotFound();
            }
            return View(courseLevel1);
        }

        // POST: CourseLevel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseLevelId,Level")] CourseLevel courseLevel1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseLevel1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(courseLevel1);
        }

        // GET: CourseLevel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseLevel courseLevel1 = db.CourseLevels.Find(id);
            if (courseLevel1 == null)
            {
                return HttpNotFound();
            }
            return View(courseLevel1);
        }

        // POST: CourseLevel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseLevel courseLevel1 = db.CourseLevels.Find(id);
            db.CourseLevels.Remove(courseLevel1);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
