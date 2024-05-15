using System.ComponentModel.DataAnnotations;

namespace School.Models
{

    [MetadataType(typeof(StudentMetaData))]
    public partial class Student
    {
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
    [MetadataType(typeof(EnrollmentMetaData))]
    public partial class Enrollment
    {

    }
    [MetadataType(typeof(CourseMetaData))]
    public partial class Course
    {

    }
    [MetadataType(typeof(DepartmentMetaData))]
    public partial class Department
    {

    }

    
}