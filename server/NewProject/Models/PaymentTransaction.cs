using System;
using System.ComponentModel.DataAnnotations;

namespace IncidentManagement.Models
{
    public class PaymentTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [StringLength(30)]
        public string LoanReference { get; set; } = string.Empty;

        public DateTime TransactionDate { get; set; }

        [StringLength(5)]
        public string? TransactionCode { get; set; }

        [StringLength(20)]
        public string? ConfirmationNumber { get; set; }

        public decimal TransactionAmount { get; set; }
        public decimal PrincipalApplied { get; set; }
        public DateTime? PaidThroughDate { get; set; }
        public decimal FeesApplied { get; set; }
    }
}