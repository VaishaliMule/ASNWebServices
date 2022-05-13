using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.Models
{
    public class BatchModel
    {
        public int BatchId { get; set; }
        public int InstituteId { get; set; }
        public int CourseId { get; set; }
        [Display(Name = "Course Name")]
        public IList<SelectListItem> CourseNames { get; set; }
        public string CourseName { get; set; }

        [Display(Name = "Batch Name")]
        public string Name { get; set; }

        [Display(Name = "Start Date")]
       // [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string Start_Date { get; set; }

        [Display(Name = "End Date")]
      
        public string End_Date { get; set; }

        [Display(Name = "Admission Last Date")]
       
        public string AdmissionLast_Date { get; set; }

        [Display(Name = "InTake Capacity")]
        public int InTakeCapacity { get; set; }

        [Display(Name = "Duration")]
        public IList<SelectListItem> DurationNames { get; set; }
        public int DurationId { get; set; }
        public string Duration { get; set; }
        public string BatchMonth { get; set; }
        public int BatchYear { get; set; }
        public int Target { get; set; }

        public IEnumerable<SelectListItem> Months
        {
            get
            {
                return DateTimeFormatInfo
                       .InvariantInfo
                       .MonthNames
                       .Select((monthName, index) => new SelectListItem
                       {
                           Value = (index + 1).ToString(),
                           Text = monthName
                       });
            }
        }
    }
}