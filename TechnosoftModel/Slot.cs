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
    
    public partial class Slot
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int BatchId { get; set; }
        public Nullable<int> FacultyId { get; set; }
        public string Description { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
    
        public virtual Batch Batch { get; set; }
        public virtual Course Course { get; set; }
    }
}
