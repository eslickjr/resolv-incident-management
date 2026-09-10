using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncidentManagement.Models
{
    /// <summary>
    /// Mapped to vw_LoanInformation on the CDM database — keyless/read-only.
    /// </summary>
    public class Loan
    {
        // ── General Loan Information ──────────────────────────────────────────
        [StringLength(10)]
        public string Branch { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Class { get; set; }

        [StringLength(50)]
        public string? Account { get; set; }

        [StringLength(200)]
        public string? Codes { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(11)]
        public string? SSN { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? Cell { get; set; }

        // ── Loan Information ──────────────────────────────────────────────────
        public DateTime? LoanDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? LoanAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Proceeds { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Balance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Payoff { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Delinquency { get; set; }

        public DateTime? ChargeOffDate { get; set; }

        // ── Payment Information ───────────────────────────────────────────────
        public DateTime? FirstPayDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PaymentAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? AmountDue { get; set; }

        public DateTime? NextDueDate { get; set; }

        public DateTime? LastDueDate { get; set; }
        public DateTime? PaidLastDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PaidLastAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? NextDueAmount { get; set; }

        /// <summary>Used internally to join to payment history.</summary>
        public long? BranchAccountNumber { get; set; }
    }

    /// <summary>
    /// Mapped to vw_PaymentHistory on the CDM database — keyless/read-only.
    /// </summary>
    public class LoanPaymentHistory
    {
        public long? BranchAccountNumber { get; set; }

        public DateTime PaymentDate { get; set; }

        [StringLength(50)]
        public string? Code { get; set; }

        [StringLength(100)]
        public string? ReferenceNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Principal { get; set; }

        public DateTime? PaidThrough { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal InterestOrLateCharge { get; set; }
    }
}