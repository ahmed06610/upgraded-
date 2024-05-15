//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace School.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            this.Enrollments = new HashSet<Enrollment>();
            this.TrainerInterestedInCourses = new HashSet<TrainerInterestedInCours>();
        }
    
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public Nullable<CourseLevel1> Level { get; set; }
        public string CourseDescription { get; set; }
        public bool IsCourseActive { get; set; }
        public Nullable<RatingLevel> Rating { get; set; }
        public Nullable<int> Level2 { get; set; }
        public Nullable<int> DepartmentId { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual CourseLevel CourseLevel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrainerInterestedInCours> TrainerInterestedInCourses { get; set; }
    }
}
