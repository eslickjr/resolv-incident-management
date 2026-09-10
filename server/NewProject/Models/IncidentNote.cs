using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncidentManagement.Models
{
    public class IncidentNote
    {
        [Key]
        public int NoteId { get; set; }

        public int IncidentId { get; set; }

        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Note { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Incident Incident { get; set; } = null!;
    }
}