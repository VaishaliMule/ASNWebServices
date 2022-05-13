using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.Models
{
    public class FeesFollowupModel
    {
        public int StudentId { get; set; }
        public int InstituteId { get; set; }
        public int CourseId { get; set; }
        [Display(Name = "Course")]
        public IList<SelectListItem> CourseNames { get; set; }
        public string CourseName { get; set; }
        public int BatchId { get; set; }
        [Display(Name = "Batch")]
        public IList<SelectListItem> BatchNames { get; set; }
        public string BatchName { get; set; }
        public int AdmissionId { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }

        [Display(Name = "Student Name")]
        public string Name { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Parent Mobile")]
        public string ParentMobile { get; set; }

        [Display(Name = "Date of Admission")]
        public string DOAdmission { get; set; }
        [Display(Name = "Followup Date")]
        public string FollowupDate { get; set; }

        [Display(Name = "Follow up Comment")]
        [DataType(DataType.MultilineText)]
        public string FollowUpComment { get; set; }
       
        public IEnumerable<FeesFollowupReportByBatch_Result> FeesFollowuplist { get; set; }

        [Display(Name = "Next Follow Up Date")]
        public string NextFollowupDate { get; set; }

        [Display(Name = "Fees Amount")]
        public decimal TotalFees { get; set; }
        [Display(Name = "Paid Amount")]
        public decimal TotalPaid { get; set; }
        [Display(Name = "Balance Amount")]
        public decimal TotalBalance { get; set; }
        [Display(Name = "Discount")]
        public decimal Discount { get; set; }

        public IEnumerable<StudentFeesFollowupBy_Admissionid_Result> FeesFollowuplistofStudent { get; set; }

        [Display(Name = "Drop Out?")]
        public bool DropOut { get; set; }

        public int RejectionReasonId { get; set; }
        [Display(Name = "Reason of Drop out")]
        public IList<SelectListItem> Rejectionreasons { get; set; }
    }
}