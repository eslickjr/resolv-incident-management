using System;
using System.Collections.Generic;

namespace IncidentManagement.Models.DTOs
{
    // ── Search ────────────────────────────────────────────────────────────────

    public class SearchResultDto
    {
        public int? IncidentId { get; set; }
        public int? LoanId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? SSN { get; set; }
        public string? Phone { get; set; }
        public string Branch { get; set; } = string.Empty;
        public string? Account { get; set; }
        public int Priority { get; set; }
        public DateTime? ClosedAt { get; set; }
    }

    // ── Incident Requests ─────────────────────────────────────────────────────

    public class CreateIncidentRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? SSN { get; set; }
        public string? Phone { get; set; }
        public string? Branch { get; set; }
        public string? Account { get; set; }
    }

    public class UpdateIncidentRequest
    {
        public string? Branch { get; set; }
        public string? Phone { get; set; }
        public string? SSN { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Issue { get; set; }
        public string? Solution { get; set; }
        public string? AdditionalDetails { get; set; }

        /// <summary>Time spent on this incident in seconds — recorded on every save.</summary>
        public int TimeSpentSeconds { get; set; } = 0;

        /// <summary>True when the user is closing the ticket.</summary>
        public bool IsClosing { get; set; } = false;
    }

    public class BeaconRequest
    {
        public int TimeSpentSeconds { get; set; }
        public bool IsClosing { get; set; }
    }
}