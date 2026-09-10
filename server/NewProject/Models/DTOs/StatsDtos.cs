using System;
using System.Collections.Generic;

namespace IncidentManagement.Models.DTOs
{
    public class UserStatsDto
    {
        public string Username { get; set; } = string.Empty;
        public int TotalIncidents { get; set; }
        public int TotalOpened { get; set; }
        public int TotalClosed { get; set; }
        public double IncidentsPerHour { get; set; }
        public double IncidentsPerDay { get; set; }
        public int AvgTimePerIncidentSeconds { get; set; }
        public int TotalTimeSeconds { get; set; }
        public int AverageResolutionSeconds { get; set; }
    }

    public class IncidentsByPeriodDto
    {
        /// <summary>Label for the period — hour (1-24), day of week (1-7), or day of month (1-31).</summary>
        public string Label { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class IncidentsByTypeDto
    {
        public string Issue { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class SolutionsByIssueDto
    {
        public string Issue { get; set; } = string.Empty;
        public string Solution { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class StatsResponseDto
    {
        public UserStatsDto Summary { get; set; } = new UserStatsDto();
        public List<IncidentsByPeriodDto> IncidentsByPeriod { get; set; } = new List<IncidentsByPeriodDto>();
        public List<IncidentsByTypeDto> IncidentsByType { get; set; } = new List<IncidentsByTypeDto>();
        public List<SolutionsByIssueDto> SolutionsByIssue { get; set; } = new List<SolutionsByIssueDto>();
    }
}