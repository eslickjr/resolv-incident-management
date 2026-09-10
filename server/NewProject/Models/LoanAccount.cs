using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncidentManagement.Models
{
    public class LoanAccount
    {
        [Key]
        public int LoanId { get; set; }

        [StringLength(10)]
        public string BranchCode { get; set; } = string.Empty;

        [StringLength(5)]
        public string? LoanClass { get; set; }

        [StringLength(20)]
        public string AccountNumber { get; set; } = string.Empty;

        [StringLength(10)]
        public string? StatusCodes { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(9)]
        public string? TaxId { get; set; }

        [StringLength(200)]
        public string? StreetAddress { get; set; }

        [StringLength(15)]
        public string? MobilePhone { get; set; }

        [StringLength(15)]
        public string? HomePhone { get; set; }

        public DateTime? OriginationDate { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? NetProceeds { get; set; }
        public decimal? PrincipalBalance { get; set; }
        public decimal? PayoffAmount { get; set; }
        public decimal? DelinquentAmount { get; set; }
        public DateTime? ChargeOffDate { get; set; }
        public DateTime? FirstPaymentDate { get; set; }
        public decimal? ScheduledPayment { get; set; }
        public decimal? AmountDue { get; set; }
        public DateTime? NextDueDate { get; set; }
        public decimal? NextDueAmount { get; set; }
        public DateTime? LastDueDate { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public decimal? LastPaymentAmount { get; set; }

        // Computed reference used for payment history lookup
        [StringLength(30)]
        public string LoanReference { get; set; } = string.Empty;
    }
}