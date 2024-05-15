using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Models
{
    public class StudentStats
    {
        public int StudentId { get; set; }
        public string FullName { get; set;}
        public string imageUrl { get; set;}
        public int NumberOfCourses { get; set;}
        public decimal AverageGrade { get; set;}


    }
}