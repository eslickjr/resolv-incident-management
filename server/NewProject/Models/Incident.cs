using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncidentManagement.Models
{
    [Table("Incidents")]
    public class Incident
    {
        [Key]
        public int IncidentId { get; set; }

        [Required]
        [StringLength(10)]
        public string Branch { get; set; } = string.Empty;
        [StringLength(6)]
        public string? Account { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(11)]
        public string? SSN { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Issue { get; set; } = string.Empty;

        public string? Solution { get; set; }

        public string? AdditionalDetails { get; set; }

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? ClosedBy { get; set; }

        public DateTime? ClosedAt { get; set; }

        // Navigation properties
        public ICollection<IncidentStatTracking> StatTrackings { get; set; } = new List<IncidentStatTracking>();
    }
}
