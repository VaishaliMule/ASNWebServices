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
    using System.Collections.Generic;
    
    public partial class Vendor
    {
        public int Id { get; set; }
        public int InstitueId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Nullable<decimal> ContactNo { get; set; }
        public string BankName { get; set; }
        public string BankACNO { get; set; }
        public string ContactPerson { get; set; }
        public Nullable<decimal> ContactPersonMobile { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string IsDeleted { get; set; }
    }
}
