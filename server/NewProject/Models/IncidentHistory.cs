using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncidentManagement.Models
{
    /// <summary>
    /// Read-only historical view of incidents — populated via a view or stored procedure.
    /// Not a direct table; mapped as a keyless entity or from a DB view.
    /// </summary>
    public class IncidentHistory
    {
        public int IncidentId { get; set; }

        public DateTime CreatedAt { get; set; }

        [StringLength(4)]
        public string Branch { get; set; } = string.Empty;

        /// <summary>Account number, if applicable.</summary>
        public string? Account { get; set; }

        /// <summary>Concatenated FirstName + " " + LastName.</summary>
        [StringLength(201)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(9)]
        public string? SSN { get; set; }

        [StringLength(10)]
        public string? Phone { get; set; }

        public string Issue { get; set; } = string.Empty;

        /// <summary>The action taken (maps to Solution on the Incident).</summary>
        public string? Solution { get; set; }

        public string? AdditionalDetails { get; set; }
    }

    [Table("IncidentStatTracking")]
    public class IncidentStatTracking
    {
        [Key]
        public int StatId { get; set; }

        [Required]
        public int IncidentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        /// <summary>True if this user opened/created the incident.</summary>
        public bool Opened { get; set; } = false;

        /// <summary>Time spent on this incident in seconds.</summary>
        public int TimeSpentSeconds { get; set; } = 0;

        /// <summary>True if this user closed the incident.</summary>
        public bool Closed { get; set; } = false;

        // Navigation
        [ForeignKey(nameof(IncidentId))]
        public Incident? Incident { get; set; }
    }
}
