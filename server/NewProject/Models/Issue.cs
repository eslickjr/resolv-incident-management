// Issue.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IncidentManagement.Models
{
    public class Issue
    {
        public int IssueId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<Solution> Solutions { get; set; } = new List<Solution>();
    }
}