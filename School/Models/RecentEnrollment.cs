using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class RecentEnrollment : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var std = (Student)validationContext.ObjectInstance;
            if (std.EnrollmentDate == null) 
            {
                return new ValidationResult("Enrollment Date is Required!");
            }
            else
            {
                var age=DateTime.Today.Year-std.EnrollmentDate.Year;
                if (age > 3)
                {
                    return new ValidationResult("Only Recent enrollment is allowed!");

                }
                return ValidationResult.Success;
            }
        }
    }
}