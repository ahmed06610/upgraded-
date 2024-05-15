using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using School.Models;

namespace School.Controllers
{
    [Authorize(Roles = RoleName.Manager)]
    public class CoursesController : BaseController
    {
        private SchoolEntities1 db = new SchoolEntities1();

        // GET: Courses
        /*public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }*/

        public ActionResult Index(string SearchString)
        {
            var courses=db.Courses.Select(c=>c);
            if (!string.IsNullOrEmpty(SearchString))
            {
                courses = courses.Where(c => c.Title.Contains(SearchString));
            }
            return View(courses.ToList());
        }

        public ActionResult StudentsInCourse(string CourseTitle)
        {
            var stds = db.Enrollments
                    .Where(e => e.Course.Title == CourseTitle);
            return View(stds);
        }
        public ActionResult IndexMultipleRecords()
        {
            return View(db.Courses.ToList());
        }
        [HttpPost]
        public ActionResult IndexMultipleRecords(FormCollection formCollection)
        {
            var ids = formCollection["ID"].Split(',');
            foreach (var id in ids)
            {
                var course = db.Courses.Find(int.Parse(id));
                db.Courses.Remove(course);
                db.SaveChanges();

            }
            return View(db.Courses.ToList());
        }
/*        public ActionResult Test()
        {
            var course=db.GetCourse().ToList();
            var c= new Course();
            c.Level = CourseLevel1.Beginner;
            return View(db.Courses.ToList());
        }
*/
        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        public ActionResult GetIdCourse(string coursename)
        {
            var Id = db.Courses
                .Where(c => c.Title == coursename)
                .FirstOrDefault();

            return RedirectToAction("Details/"+ Id.CourseId);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.Level2 = new SelectList(db.CourseLevels,"CourseLevelId","Level");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId ", "DepartmentName");

            return View();
        }
        public ActionResult GetPartialNewCourse()
        {
            ViewBag.Level2 = new SelectList(db.CourseLevels, "CourseLevelId", "Level");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId ", "DepartmentName");

            return PartialView("_PartialNewCourse");
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Level2 = new SelectList(db.CourseLevels, "CourseLevelId", "Level");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId ", "DepartmentName");

            return View(course);
        }
        public JsonResult GetSearchValue(string search)
        {
            var suggestedcourses = db.Courses
                                    .Where(c => c.Title.Contains(search))
                                    .Select(c=>new
                                    {
                                        Title=c.Title
                                    }).ToList();
            return new JsonResult{Data= suggestedcourses, JsonRequestBehavior=JsonRequestBehavior.AllowGet};
        }


        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.Level2 = new SelectList(db.CourseLevels, "CourseLevelId", "Level", course.Level2);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId ", "DepartmentName", course.DepartmentId);


            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            ViewBag.Level2 = new SelectList(db.CourseLevels, "CourseLevelId", "Level", course.Level2);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId ", "DepartmentName", course.DepartmentId);
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteConfirmedJSON(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return Json(true,JsonRequestBehavior.AllowGet);
        }
        Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
        ChartArea ca = new ChartArea();

        void ApplyVanillaTheme()
        {
            chart.Palette = ChartColorPalette.SemiTransparent;
            chart.BorderlineColor = System.Drawing.ColorTranslator.FromHtml("#000");
            chart.BorderlineWidth = 2;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            ca.AxisY.MajorGrid.Enabled = false;
            ca.AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas.Add(ca);
        }

        void ApplyBlueTheme()
        {
            chart.BackColor = System.Drawing.ColorTranslator.FromHtml("#D3DFF0");
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BackSecondaryColor = Color.White;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderlineWidth = 2;
            chart.Palette = ChartColorPalette.BrightPastel;

            ca.BorderColor = Color.FromArgb(64, 165, 191, 228);
            ca.BackGradientStyle = GradientStyle.TopBottom;
            ca.BackSecondaryColor = Color.White;
            ca.BorderColor = Color.FromArgb(64, 64, 64, 64);
            ca.BorderDashStyle = ChartDashStyle.Solid;
            ca.ShadowColor = Color.Transparent;

            ca.AxisY.MajorGrid.Enabled = false;
            ca.AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas.Add(ca);
        }

        public ActionResult Chart()
        {

            chart.Width = 850;
            chart.Height = 500;
            chart.EnableTheming = true;

            ApplyVanillaTheme();

            // Dummy data
            var courses = db.Courses.ToList();

            chart.Series.Add(new Series("Data"));
            chart.Series["Data"].ChartType = SeriesChartType.Column;

            foreach (var course in courses)
            {

                // Add each point and set its Label
                int ptIdx = chart.Series["Data"].Points.AddXY(
                     course.Title,
                     course.Credits);

                DataPoint pt = chart.Series["Data"].Points[ptIdx];
                pt.Label = "#VALY";
                //pt.Font = new System.Drawing.Font("Arial", 15f, FontStyle.Bold);
                //pt.LabelForeColor = Color.Green;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                return File(ms.ToArray(), "image/png");
            }
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
