using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using School.Models;

namespace School.Controllers
{
    [Authorize(Roles = RoleName.Manager)]

    public class StudentsController : BaseController
    {
        private SchoolEntities1 db = new SchoolEntities1();

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,FirstName,LastName,gender,EnrollmentDate,ImagePath")] Student student,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Content/images/Students/") +file.FileName);
                    student.ImagePath = file.FileName;
                }
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        public ActionResult UpdateGender(int studentid,string gender)
        {
            Student std = db.Students
                            .Where(s => s.StudentId==studentid)
                            .FirstOrDefault();

            if (std != null)
            {
                Enum.TryParse(gender, out GenderType mygender);
                std.gender = mygender;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { success = false, responseText = "" }, JsonRequestBehavior.AllowGet);

                }
            }


            return Json(new { success = true, responseText = "Gender Updated" }, JsonRequestBehavior.AllowGet);

        }


        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,FirstName,LastName,gender,EnrollmentDate,ImagePath")] Student student,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if(!string.IsNullOrWhiteSpace(student.ImagePath))
                    {


                        var rootFolder = "~/Content/images/Students/";
                        var authorsFile = @Url.Content(student.ImagePath);

                        string fullPath = Request.MapPath(rootFolder + authorsFile);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                       
                    }
                         file.SaveAs(HttpContext.Server.MapPath("~/Content/images/Students/") + file.FileName);
                         student.ImagePath = file.FileName;
                    
                }
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteConfirmedJSON(int id)
        {
            var std = db.Students.Find(id);
            db.Students.Remove(std);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
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
