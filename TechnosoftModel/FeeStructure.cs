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
    
    public partial class FeeStructure
    {
        public int id { get; set; }
        public int courseid { get; set; }
        public int duratioinid { get; set; }
        public decimal permonthamt { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual Duration Duration { get; set; }
    }
}
