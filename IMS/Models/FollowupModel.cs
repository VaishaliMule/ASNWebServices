using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.Models
{
    public class FollowupModel
    {
        public int StudentId { get; set; }
        public int InstituteId { get; set; }
        public int CourseId { get; set; }
        [Display(Name = "Course")]
        public IList<SelectListItem> CourseNames { get; set; }
        public string CourseName { get; set; }
        public int EnquiryId { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }

        [Display(Name = "Student Name")]
        public string Name { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Parent Mobile")]
        public string ParentMobile { get; set; }

        [Display(Name = "Date of Enquiry")]
        public string DOEnquiry { get; set; }
        [Display(Name = "Followup Date")]
        public string FollowupDate { get; set; }

        [Display(Name = "Not Interested?")]
        public bool Interested { get; set; }

        [Display(Name = "Follow up Comment")]
        [DataType(DataType.MultilineText)]
        public string FollowUpComment { get; set; }

        [Display(Name = "From Date")]
        public string Start_Date { get; set; }

        [Display(Name = "To Date")]
        public string End_Date { get; set; }
        public IEnumerable<AllEnquiryFollowupReportByInstituteId_Result> EnquiryFollowuplist { get; set; }
        public IEnumerable<FollowupofStudent_enquiryid_instituteid_Result> EnquiryFollowuplistofStudent { get; set; }

        public int RejectionReasonId { get; set; }
        [Display(Name = "Reason of Rejection")]
        public IList<SelectListItem> Rejectionreasons { get; set; }

        [Display(Name = "Next Follow Up Date")]
        public string NextFollowupDate { get; set; }
    }
}