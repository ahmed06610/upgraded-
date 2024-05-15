using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace School.Models
{
    public class CourseMetaData
    {
        [Required(ErrorMessage = "Please Enter The Title!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please Enter The Credits!")]
        [Range(2, 6, ErrorMessage = "Please Enter a Number Between Two and Six!")]

        public int Credits { get; set; }
        [Display(Name ="Level")]
        [Required(ErrorMessage = "Please Enter Level")]
        public Nullable<int> Level2 { get; set; }
        [Display(Name = "Description")]
        public string CourseDescription { get; set; }
        [EnumDataType(typeof(RatingLevel), ErrorMessage = "Please Enter Rate")]
        [Required(ErrorMessage = "Please Enter Rate")]
        public Nullable<RatingLevel> Rating { get; set; }
    }

    public class StudentMetaData
    {
        [Required(ErrorMessage ="Enter The First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Enter The Last Name")]
        public string LastName { get; set; }


        [RecentEnrollment]
        public System.DateTime EnrollmentDate { get; set; }

    }

    public class EnrollmentMetaData
    {
        [Range(0,100, ErrorMessage = "Please Enter a Number Between Zero and One Hundered!")]
        public Nullable<decimal> Grade { get; set; }
    }

    public class DepartmentMetaData
    {
        [Display(Name = "Name of Department")]
        public string DepartmentName { get; set; }
    }
}