using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMS.Models
{
    public class CourseFeesModel
    {
        public int CourseId { get; set; }

        [Display(Name ="Course Name")]
        public string CourseName { get; set; }

        [Display(Name = "Number of Installments")]
        public int NoOfInstallment { get; set; }

        [Display(Name = "Course Fee")]
        public decimal FeesAmount { get; set; }
        public int InstituteId { get; set; }
        public IEnumerable<AllCourseFeesByInstituteId_Result> CourseFees { get; set; }

        public string[] DyanamicTextbox { get; set; }

        public int CourseFeesId { get; set; }

        public string ComboOrSingle { get; set; }
    }
}