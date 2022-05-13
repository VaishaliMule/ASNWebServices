using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMS.Models
{
    public class SPCourseModelByInstitute
    {
        [Display(Name ="Sr. No.")]
        public string SN { get; set; }
        public int Id { get; set; }
        public int InstituteId { get; set; }
        [Display(Name = "Course Type")]
        public string CourseType { get; set; }
        public int CourseTypeId { get; set; }
        
        [Display(Name = "Code")]
        public string Code { get; set; }
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [Display(Name = "Duration")]
        public string DurationInMonths { get; set; }
        public int DurationId { get; set; }
        
        [Display(Name = "Description")]
        public string Description { get; set; }
       
        [Display(Name = "Course Pattern")]
        public string IsCombo { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

    }
}