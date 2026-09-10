using System.ComponentModel.DataAnnotations;

namespace IncidentManagement.Models
{
    public class CallTip
    {
        [Key]
        public int TipId { get; set; }

        public int IssueId { get; set; }

        [StringLength(500)]
        public string Tip { get; set; } = string.Empty;

        public int SortOrder { get; set; } = 0;

        public Issue Issue { get; set; } = null!;
    }
}