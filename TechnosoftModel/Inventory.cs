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
    
    public partial class Inventory
    {
        public int Id { get; set; }
        public Nullable<int> InstitueId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceBrand { get; set; }
        public string Details { get; set; }
        public Nullable<decimal> PurchasePrice { get; set; }
    }
}
