using Microsoft.AspNet.Identity;
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace School.Controllers
{
    [Authorize(Roles = RoleName.Trainer)]

    public class InterestsController : Controller
    {
        private SchoolEntities1 db = new SchoolEntities1();

        // GET: Interests
        public ActionResult Index()
        {
            var CurrentTrainerId=User.Identity.GetUserId();

            var TrainerDepartmentId=db.Trainers
                                .Where(x=>x.AspNetUserId== CurrentTrainerId)
                                .Select(x=>x.DepartmentId)
                                .FirstOrDefault();

            var CoursesInThisDepartment = db.Courses
                .Where(c => c.DepartmentId == TrainerDepartmentId)
                .Select(c => c);

            return View(CoursesInThisDepartment.ToList());
        }

        public ActionResult AddRemoveInterestJSON(int courseid, bool status)
        {
            string currentUserId = User.Identity.GetUserId();

            /* if (!status)
             {
                 var query = (from tc in db.TrainerInterestedInCourses
                              where tc.Course == courseid && tc.Trainer.AspNetUserId == currentUserId
                              select tc).FirstOrDefault();

                 if (query != null)
                 {
                     var trainerInterestedInCours = db.TrainerInterestedInCourses.Find(query.TrainerInterested);
                     db.TrainerInterestedInCourses.Remove(trainerInterestedInCours);
                     db.SaveChanges();
                 }

             }
             else
             {
                 var query = (from t in db.Trainers
                              where t.AspNetUserId == currentUserId
                              select t).First();

                 TrainerInterestedInCours tc = new TrainerInterestedInCours();
                 tc.Course = courseid;
                 tc.TrainerId = query.TrainerId;
                 db.TrainerInterestedInCourses.Add(tc);
                 db.SaveChanges();

             }*/

            if (!status)
            {
                var q=db.TrainerInterestedInCourses
                    .Where(tc=>tc.Course==courseid && tc.Trainer.AspNetUserId==currentUserId)
                    .Select(tc=>tc).FirstOrDefault();

                if (q!=null)
                {
                    db.TrainerInterestedInCourses.Remove(q);
                    db.SaveChanges();
                }
            }
            else
            {
                var t = db.Trainers
                    .Where(tr => tr.AspNetUserId == currentUserId)
                    .Select(tr=>tr).FirstOrDefault();

                var tc = new TrainerInterestedInCours();
                tc.Course = courseid;
                tc.TrainerId = t.TrainerId;
                db.TrainerInterestedInCourses .Add(tc);
                db.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public bool IsChosen(int CourseId)
        {
            var CurrentTrainerId=User.Identity.GetUserId();
            var q = db.TrainerInterestedInCourses
                .Where(t => t.Trainer.AspNetUserId == CurrentTrainerId && t.Course == CourseId)
                .Select(t => t);
            if(q.Count() > 0) 
            { 
                return true;
            }
            return false;

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