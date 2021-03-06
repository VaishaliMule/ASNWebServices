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
    
    public partial class EWalletTransation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EWalletTransation()
        {
            this.PaytmPaymentResponses = new HashSet<PaytmPaymentResponse>();
            this.RazorPayments = new HashSet<RazorPayment>();
        }
    
        public int Id { get; set; }
        public int EWalletId { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> LearnerId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public string AdminRemark { get; set; }
        public string PaymentType { get; set; }
        public Nullable<decimal> Discount { get; set; }
    
        public virtual EWallet EWallet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaytmPaymentResponse> PaytmPaymentResponses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RazorPayment> RazorPayments { get; set; }
    }
}
