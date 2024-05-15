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
    public class TrainersController : Controller
    {
        private SchoolEntities1 db = new SchoolEntities1();

        // GET: Trainers
        public ActionResult Index()
        {
            var trainers = db.Trainers.Include(t => t.AspNetUser).Include(t => t.Department);
            return View(trainers.ToList());
        }

        // GET: Trainers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainers.Find(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

        // GET: Trainers/Create
        public ActionResult Create()
        {
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Trainers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrainerId,FirstName,LastName,DepartmentId,AspNetUserId")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                db.Trainers.Add(trainer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "UserName", trainer.AspNetUserId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", trainer.DepartmentId);
            return View(trainer);
        }

        // GET: Trainers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainers.Find(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "UserName", trainer.AspNetUserId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", trainer.DepartmentId);
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Trainer trainer, HttpPostedFileBase files)
        {
            string fileName = null;
            // Verify that the user selected a file
            if (files != null && files.ContentLength > 0)
            {
                // extract only the filename
                 fileName = Path.GetFileName(files.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/Content/Resumes"), fileName);
                files.SaveAs(path);
            }

                if (ModelState.IsValid)
                {
                    if(fileName!=null)
                        trainer.ResumePath = fileName;

                    db.Entry(trainer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", trainer.DepartmentId);
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "UserName", trainer.AspNetUserId);
            return View(trainer);
        }

        public string GetInterests(int TrainerId)
        {
            var t=db.TrainerInterestedInCourses
                .Where(tr=>tr.TrainerId==TrainerId).ToList();

            string interests = "";
            foreach (var tr in t)
            {
                interests += tr.Course1.Title + " | ";
            }
            return interests;
        
        
        }

        // GET: Trainers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainers.Find(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trainer trainer = db.Trainers.Find(id);
            db.Trainers.Remove(trainer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public FilePathResult DownloadCV(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            string contentType = "";

            if (ext == ".doc")
            {
                contentType = "application/msword";
            }
            else if (ext == ".docx")
            {
                contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }
            else
            {
                contentType = "application/pdf";
            }

            var path = Path.Combine(Server.MapPath("~/Content/Resumes"), fileName);

            return new FilePathResult(path, contentType);
        }

        public JsonResult DrawCoursesInterestsGoogleChart()
        {
            List<CourseInterestsStats> CoursesInterestsStats = new List<CourseInterestsStats>();
            var Coursegroups = db.TrainerInterestedInCourses.GroupBy(g => g.Course1.Title);
            foreach (var group in Coursegroups)
            {
                CoursesInterestsStats.Add(new CourseInterestsStats
                {
                    Course = group.Key,
                    Number_Of_Trainers = group.Count()
                });
            }

            return Json(CoursesInterestsStats);
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
