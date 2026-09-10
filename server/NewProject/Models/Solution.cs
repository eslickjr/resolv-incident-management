// Solution.cs
using System.ComponentModel.DataAnnotations;

namespace IncidentManagement.Models
{
    public class Solution
    {
        public int SolutionId { get; set; }

        public int IssueId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public Issue Issue { get; set; } = null!;
    }
}