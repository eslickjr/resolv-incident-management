using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncidentManagement.Models
{
    public class LoanHistory
    {
        public DateTime LoanDateTime { get; set; }

        [StringLength(10)]
        public string Branch { get; set; } = string.Empty;

        [StringLength(50)]
        public string? AccountNumber { get; set; }

        [StringLength(201)]
        public string FullName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? LoanAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Proceeds { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PaymentAmount { get; set; }

        [StringLength(100)]
        public string? ClosedDate { get; set; }
    }
}