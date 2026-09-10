using System.Collections.Generic;

namespace IncidentManagement.Models.DTOs
{
    public class WebhookCallRequest
    {
        public string Phone { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class CallMatchResult
    {
        public string MatchType { get; set; } = string.Empty;
        public int? IncidentId { get; set; }
        public string? Branch { get; set; }
        public string? Account { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<CallMatchItem> Matches { get; set; } = new List<CallMatchItem>();
    }

    public class CallMatchItem
    {
        public string Source { get; set; } = string.Empty;
        public int? IncidentId { get; set; }
        public string? Branch { get; set; }
        public string? Account { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Issue { get; set; }
        public string? ClosedAt { get; set; }
        public string? SSN { get; set; }
        public bool HasLoan { get; set; }
    }
}