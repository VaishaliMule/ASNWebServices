//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TechnosoftModel
{
    using System;
    
    public partial class StudentFeesReceiptBy_Courseid_batchid_Result
    {
        public Nullable<long> SN { get; set; }
        public int Id { get; set; }
        public int InstituteId { get; set; }
        public int CourseId { get; set; }
        public int BatchId { get; set; }
        public Nullable<int> StudentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string ParentMobile { get; set; }
        public string NameOnCertificate { get; set; }
        public Nullable<System.DateTime> AdmissionDate { get; set; }
        public Nullable<decimal> TotalFees { get; set; }
        public Nullable<decimal> TotalPaid { get; set; }
        public Nullable<decimal> TotalBalance { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<bool> IsComplete { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string Photo { get; set; }
        public string Signature { get; set; }
        public string PhotoFileName { get; set; }
        public string SignFileNAme { get; set; }
        public Nullable<bool> IsComboCourseStudent { get; set; }
        public Nullable<int> ComboCourseId { get; set; }
    }
}
