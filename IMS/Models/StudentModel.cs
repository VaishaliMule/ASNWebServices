using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.Models
{
    public class StudentModel
    {
        public int StudentId { get; set; }
        public int InstituteId { get; set; }
        public int CourseId { get; set; }
        [Display(Name = "Course")]
        public IList<SelectListItem> CourseNames { get; set; }
        public int BatchId { get; set; }
        [Display(Name = "Batch")]
        public IList<SelectListItem> BatchNames { get; set; }
        [Display(Name ="Course Name")]
        public string CourseName { get; set; }
        [Display(Name = "Batch Name")]
        public string BatchName { get; set; }
        public int EnquiryId { get; set; }
        public int AdmissionId { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]

        public string LastName { get; set; }

        [Display(Name = "Gender")]

       // public GenderList Gender { get; set; }
        public string Gender { get; set; }

        [Display(Name = "Email")]

        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid email Id")]
        public string Email { get; set; }

        [Display(Name = "Date of Birth")]
        // [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public string DOB { get; set; }

        [Display(Name = "Qualification")]

        public List<SelectListItem> QualificationNames { get; set; }
        public int? QualificationId { get; set; }
        public string Qualification { get; set; }

        [Display(Name = "Marital Status")]
        public string MarritalStatus { get; set; }

        [Display(Name = "Aadhar Number")]
        
        public string Aadhar { get; set; }

        [Display(Name = "Mother Tangue")]
        public List<SelectListItem> LanguageNames { get; set; }
        public int? LangauageId { get; set; }
        public string Langauage { get; set; }

        [Display(Name = "Mobile")]

        public string Mobile { get; set; }

        [Display(Name = "Parent Mobile")]
        public string ParentMobile { get; set; }

        [Display(Name = "Address")]
        public string Address {get; set;}

        [Display(Name = "Country")]
      
        public IList<SelectListItem> CountryNames { get; set; }

        public int CountryId { get; set; }

        [Display(Name = "State")]
      
        public IList<SelectListItem> StateNames { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }

        [Display(Name = "District")]
     
        public IList<SelectListItem> DistrictNames { get; set; }
        public int DistrictId { get; set; }
        public string District { get; set; }
        [Display(Name = "Tahsil/ Block")]
       
        public IList<SelectListItem> TahsilNames { get; set; }
        public int TahsilId { get; set; }
        public string Tahsil { get; set; }

        [Display(Name = "Location")]
        
        public IList<SelectListItem> AreaNames { get; set; }
        public int AreaId { get; set; }
        public string Area { get; set; }
        [Display(Name = "Pin Code")]
        [MaxLength(6)]
        public string PinCode { get; set; }
        public string PhysicallyChallenged { get; set; }

        [Display(Name = "Reference")]
        public List<SelectListItem> SourceOfInformation { get; set; }
        public int? InformationId { get; set; }
        public string Reference { get; set; }

        [Display(Name = "Followup Days")]
        public int FollowupDays { get; set; }

        [Display(Name = "Followup Prefered Time")]
        public string FollowupPreferedTime { get; set; }

        [Display(Name = "Followup Description")]
        public string FollowupDescription { get; set; }

        [Display(Name ="No of Installment")]
        public IList<SelectListItem> CourseFees { get; set; }
        public int CourseFeesId { get; set; }

        public int Installment { get; set; }
        public int FeesId { get; set; }
        [Display(Name = "Amount")]
        public string Paid { get; set; }
        public string Discount { get; set; }
        [Display(Name = "Mode of Payment")]
        public string ModeOfPayment { get; set; }
        [Display(Name = "Total Fees")]
        public string TotalFees { get; set; }
        [Display(Name = "Total Paid Fees")]
        public string TotalPaidFees { get; set; }
        [Display(Name = "Balance Amount")]
        public string BalanceAmount { get; set; }

        [Display(Name = "Identity Proof")]
        public IList<SelectListItem> IdentityProofs { get; set; }
        public int IdentityProofId { get; set; }

        [Display(Name = "Name On Certificate")]
        public string NameOnCertificate { get; set; }
        public string Fullname { get; set; }
        [Display(Name = "Remark")]
        public string Remark { get; set; }
        public string PaidDate { get; set; }
        public IEnumerable<AllEnquiriesOfStudentByInstituteId_Result> StudentsEnquires { get; set; }
        public IEnumerable<AllAdmissionOfStudentByInstituteId_Result> StudentsAdmissions { get; set; }
        public IEnumerable<StudentFeesReceiptBy_Courseid_batchid_Result> StudentsFeesReceipts { get; set; }
        public string PhotoUpload { get; set; }
        public HttpPostedFileBase PhotoImageFile { get; set; }
        public string Photofilename { get; set; }
        public string SignUpload { get; set; }
        public HttpPostedFileBase SignImageFile { get; set; }
        public string Signfilename { get; set; }
        public IEnumerable<DataForPrintReceipt_studentId_batchId_Result> StudentsPrintFeesReceipts { get; set; }
        public string InstituteName { get; set; }
        public string InstituteAddress { get; set; }
        public string ProofUTRUpload { get; set; }
        public HttpPostedFileBase ProofUTRImageFile { get; set; }
        public string ProofUTRfilename { get; set; }
        public string UTRNo { get; set; }
        public string TransactionDate { get; set; }
        public string CustomerDetail { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string ProofUpload { get; set; }
        public HttpPostedFileBase ProofImageFile { get; set; }
        public string Prooffilename { get; set; }

        public string LogoUpload { get; set; }
        public HttpPostedFileBase LogoImageFile { get; set; }
        public string Logofilename { get; set; }
        public string Disability { get; set; }
        public IEnumerable<AllActiveBatchesOfSubCourses_Result> subcoursesWithBAtches { get; set; }
    }
}