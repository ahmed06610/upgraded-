using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace School.Controllers
{
    public class BaseController : Controller
    {
        private SchoolEntities1 db = new SchoolEntities1();
        static  List<CourseStats> Stats = null;
        public BaseController()
        {
            Stats = new List<CourseStats>();
            var coursegroups = db.Enrollments.GroupBy(e => e.Course.Title).Select(g=>new CourseStats
            {
                CourseTitle = g.Key,
                CourseEnrollments=g.Count(),
            }).ToList();
            /*foreach (var coursegroup in coursegroups)
            {
            Stats.Add(new CourseStats { CourseTitle = coursegroup.Key, CourseEnrollments = coursegroup.Count()});
            }*/

            ViewBag.CourseStats= coursegroups;
            //////////////////////////////////////////////////////////////////////////

            var StudentStats = db.Enrollments
                .GroupBy(e => e.StudentId)
                .Select(g => new StudentStats
                {
                    StudentId = g.Key,
                    FullName = g.FirstOrDefault().Student.FirstName+" "+ g.FirstOrDefault().Student.LastName,
                    NumberOfCourses = g.Count(),
                    AverageGrade = (decimal)g.Average(e => e.Grade),
                    imageUrl = g.FirstOrDefault().Student.ImagePath,
                }).OrderByDescending(s=>s.AverageGrade).Take(3).ToList();

            ViewBag.Stds = StudentStats;
        }

        // GET: Base

    }
}