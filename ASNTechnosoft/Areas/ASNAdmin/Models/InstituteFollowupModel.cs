
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnosoftModel;

namespace ASNTechnosoft.Areas.ASNAdmin.Models
{
    public class InstituteFollowupModel
    {
        
        public int InstituteId { get; set; }

        [Display(Name = "Institute Names")]
        public string InstituteName { get; set; }

        [Display(Name = "Owner Names")]
        public string OwnerName { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Alternate Mobile")]
        public string AlternateMobile { get; set; }

        [Display(Name = "Date of Registration")]
        public string DORegistration { get; set; }
        [Display(Name = "Followup Date")]
        public string FollowupDate { get; set; }

        [Display(Name = "Not Interested?")]
        public bool Interested { get; set; }

        [Display(Name = "Follow up Comment")]
        [DataType(DataType.MultilineText)]
        public string FollowUpComment { get; set; }

        //[Display(Name = "From Date")]
        //public string Start_Date { get; set; }

        //[Display(Name = "To Date")]
        //public string End_Date { get; set; }
        //public IEnumerable<GetAllEnquiryFollowupReportByInstituteId_Result> EnquiryFollowuplist { get; set; }
        public IEnumerable<InstituteFollowupHistory> InstituteFollowuplist { get; set; }

        public int RejectionReasonId { get; set; }
        [Display(Name = "Reason of Rejection")]
        public IList<SelectListItem> Rejectionreasons { get; set; }

        [Display(Name = "Next Follow Up Date")]
        public string NextFollowupDate { get; set; }
    }
}