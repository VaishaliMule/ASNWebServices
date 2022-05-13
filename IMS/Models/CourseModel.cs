using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.Models
{
    public class CourseModel
    {
        public int CourseId { get; set; }
        [Display(Name = "Course Type")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please select Course Type")]
        public IList<SelectListItem> CourseTypeNames { get; set; }
        public int CourseTypeId { get; set; }
        public string CourseType { get; set; }
        public int InstituteId { get; set; }
        [Display(Name = "Course Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Course Code is required")]
        public string Code { get; set; }
        [Display(Name = "Course Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Course Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Display(Name = "Duration")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please select Duration")]
        public IList<SelectListItem> DurationNames { get; set; }
        public int DurationId { get; set; }
        public string Duration { get; set; }

        [Display(Name = "Course Pattern")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select Course Pattern")]
        public string CoursePattern { get; set; }
        [Display(Name = "Course Selection Limit")]
        [Range(2, 12, ErrorMessage = "Course limit between 2 to 12")]
        public int CourseSelectionLimit { get; set; }
        public List<SelectListItem>  AllCoursesForCombo { get; set; }
        public IEnumerable<SubcoursesOfCombo_Result> SubCourses { get; set; }
    }
}