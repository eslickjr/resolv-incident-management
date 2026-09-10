using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IncidentManagement.Data;
using IncidentManagement.Models;
using IncidentManagement.Models.DTOs;

namespace IncidentManagement.Services
{
    public enum StatsPeriod { Day, Week, Month }

    public interface IStatsService
    {
        Task<StatsResponseDto> GetAggregateStatsAsync(StatsPeriod period);
        Task<StatsResponseDto> GetStatsAsync(string username, StatsPeriod period, string? filterUsername = null);
        Task<List<UserStatsDto>> GetAllUserStatsAsync(StatsPeriod period);
        Task<bool> IsAdminAsync(string username);
    }

    public class StatsService : IStatsService
    {
        private readonly IncidentDbContext _db;
        private static readonly TimeZoneInfo EasternTz = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        private static readonly HashSet<string> Admins = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "admin@company.com",
            "jsmith"
        };

        public StatsService(IncidentDbContext db) => _db = db;

        public Task<bool> IsAdminAsync(string username) =>
            Task.FromResult(Admins.Contains(username));

        private static DateTime ToEastern(DateTime utc) =>
            TimeZoneInfo.ConvertTimeFromUtc(utc, EasternTz);

        private static bool IsBusinessHours(DateTime utc)
        {
            var et = ToEastern(utc);
            if (et.DayOfWeek == DayOfWeek.Saturday || et.DayOfWeek == DayOfWeek.Sunday)
                return false;
            var timeOfDay = et.TimeOfDay;
            return timeOfDay >= new TimeSpan(8, 30, 0) && timeOfDay < new TimeSpan(17, 0, 0);
        }

        private static (double IncidentsPerHour, double IncidentsPerDay) CalculateProductivity(
            List<Incident> incidents)
        {
            // Filter to business hours incidents only
            var bizHoursIncidents = incidents
                .Where(i => IsBusinessHours(i.CreatedAt))
                .ToList();

            if (bizHoursIncidents.Count == 0)
                return (0, 0);

            // Get distinct worked days (days with at least one business hours incident)
            var workedDays = bizHoursIncidents
                .Select(i => ToEastern(i.CreatedAt).Date)
                .Distinct()
                .Count();

            // Each worked day has 8.5 eligible hours (8:30am - 5pm)
            double totalEligibleHours = workedDays * 8.5;

            double perHour = totalEligibleHours > 0
                ? Math.Round(bizHoursIncidents.Count / totalEligibleHours, 2)
                : 0;

            double perDay = workedDays > 0
                ? Math.Round((double)bizHoursIncidents.Count / workedDays, 2)
                : 0;

            return (perHour, perDay);
        }

        public async Task<StatsResponseDto> GetAggregateStatsAsync(StatsPeriod period)
        {
            var cutoff = GetCutoff(period);

            var incidents = await _db.Incidents
                .Where(i => i.CreatedAt >= cutoff)
                .ToListAsync();

            var allStats = await _db.IncidentStatTrackings.ToListAsync();

            var relevantStats = allStats
                .Where(s => incidents.Any(i => i.IncidentId == s.IncidentId))
                .ToList();

            int totalIncidents = incidents.Count;
            int totalOpened    = incidents.Count;
            int totalClosed    = incidents.Count(i => i.ClosedAt != null);
            int totalTime      = relevantStats.Sum(s => s.TimeSpentSeconds);
            int avgResolution  = totalClosed > 0
                ? relevantStats.Where(s => incidents.Any(i => i.IncidentId == s.IncidentId && i.ClosedAt != null))
                               .Sum(s => s.TimeSpentSeconds) / totalClosed
                : 0;
            int avgTimePerInc  = totalIncidents > 0 ? totalTime / totalIncidents : 0;

            var (perHour, perDay) = CalculateProductivity(incidents);
            var byPeriod = BuildPeriodData(incidents, period);

            var byType = incidents
                .GroupBy(i => i.Issue)
                .Select(g => new IncidentsByTypeDto { Issue = g.Key, Count = g.Count() })
                .ToList();

            var byIssue = incidents
                .Where(i => i.Solution != null && i.Issue != null)
                .GroupBy(i => new { i.Issue, i.Solution })
                .Select(g => new SolutionsByIssueDto
                {
                    Issue    = g.Key.Issue!,
                    Solution = g.Key.Solution!,
                    Count    = g.Count()
                })
                .ToList();

            return new StatsResponseDto
            {
                Summary = new UserStatsDto
                {
                    Username                 = "All Users",
                    TotalIncidents           = totalIncidents,
                    TotalOpened              = totalOpened,
                    TotalClosed              = totalClosed,
                    IncidentsPerHour         = perHour,
                    IncidentsPerDay          = perDay,
                    AvgTimePerIncidentSeconds = avgTimePerInc,
                    TotalTimeSeconds         = totalTime,
                    AverageResolutionSeconds = avgResolution
                },
                IncidentsByPeriod = byPeriod,
                IncidentsByType   = byType,
                SolutionsByIssue  = byIssue
            };
        }

        public async Task<StatsResponseDto> GetStatsAsync(
            string username, StatsPeriod period, string? filterUsername = null)
        {
            var targetUser = await IsAdminAsync(username) && filterUsername != null
                ? filterUsername : username;

            var cutoff = GetCutoff(period);

            var statRows = await _db.IncidentStatTrackings
                .Where(s => s.Username == targetUser)
                .ToListAsync();

            var incidentIds = statRows.Select(s => s.IncidentId).ToHashSet();

            var incidents = await _db.Incidents
                .Where(i => incidentIds.Contains(i.IncidentId) && i.CreatedAt >= cutoff)
                .ToListAsync();

            var relevantStats = statRows
                .Where(s => incidents.Any(i => i.IncidentId == s.IncidentId))
                .ToList();

            int totalIncidents = incidents.Count;
            int totalOpened    = incidents.Count(i => i.CreatedBy == targetUser);
            int totalClosed    = incidents.Count(i => i.ClosedBy == targetUser);
            int totalTime      = relevantStats.Sum(s => s.TimeSpentSeconds);
            int avgTimePerInc  = totalIncidents > 0 ? totalTime / totalIncidents : 0;

            var (perHour, perDay) = CalculateProductivity(incidents);
            var byPeriod = BuildPeriodData(incidents, period);

            var byType = incidents
                .GroupBy(i => i.Issue)
                .Select(g => new IncidentsByTypeDto { Issue = g.Key, Count = g.Count() })
                .ToList();

            var byIssue = incidents
                .Where(i => i.Solution != null && i.Issue != null)
                .GroupBy(i => new { i.Issue, i.Solution })
                .Select(g => new SolutionsByIssueDto
                {
                    Issue    = g.Key.Issue!,
                    Solution = g.Key.Solution!,
                    Count    = g.Count()
                })
                .ToList();

            return new StatsResponseDto
            {
                Summary = new UserStatsDto
                {
                    Username                 = targetUser,
                    TotalIncidents           = totalIncidents,
                    TotalOpened              = totalOpened,
                    TotalClosed              = totalClosed,
                    IncidentsPerHour         = perHour,
                    IncidentsPerDay          = perDay,
                    AvgTimePerIncidentSeconds = avgTimePerInc,
                    TotalTimeSeconds         = totalTime
                },
                IncidentsByPeriod = byPeriod,
                IncidentsByType   = byType,
                SolutionsByIssue  = byIssue
            };
        }

        public async Task<List<UserStatsDto>> GetAllUserStatsAsync(StatsPeriod period)
        {
            var cutoff = GetCutoff(period);

            var allStats = await _db.IncidentStatTrackings.ToListAsync();
            var allIncidents = await _db.Incidents
                .Where(i => i.CreatedAt >= cutoff)
                .ToListAsync();

            return allStats
                .GroupBy(s => s.Username)
                .Select(g =>
                {
                    var incidentIds = g.Select(s => s.IncidentId).ToHashSet();
                    var incidents   = allIncidents.Where(i => incidentIds.Contains(i.IncidentId)).ToList();
                    int total       = incidents.Count;
                    int opened      = incidents.Count(i => i.CreatedBy == g.Key);
                    int closed      = incidents.Count(i => i.ClosedBy == g.Key);
                    int time        = g.Sum(s => s.TimeSpentSeconds);
                    int avgPerInc   = total > 0 ? time / total : 0;
                    var (perHour, perDay) = CalculateProductivity(incidents);

                    return new UserStatsDto
                    {
                        Username                 = g.Key,
                        TotalIncidents           = total,
                        TotalOpened              = opened,
                        TotalClosed              = closed,
                        IncidentsPerHour         = perHour,
                        IncidentsPerDay          = perDay,
                        AvgTimePerIncidentSeconds = avgPerInc,
                        TotalTimeSeconds         = time
                    };
                })
                .ToList();
        }

        private static DateTime GetCutoff(StatsPeriod period) => period switch
        {
            StatsPeriod.Day   => DateTime.UtcNow.AddHours(-24),
            StatsPeriod.Week  => DateTime.UtcNow.AddDays(-7),
            StatsPeriod.Month => DateTime.UtcNow.AddDays(-31),
            _                 => DateTime.UtcNow.AddDays(-31)
        };

        private static List<IncidentsByPeriodDto> BuildPeriodData(
            List<Incident> incidents, StatsPeriod period)
        {
            if (period == StatsPeriod.Day)
            {
                var counts = incidents.GroupBy(i => i.CreatedAt.Hour)
                    .ToDictionary(g => g.Key, g => g.Count());
                return Enumerable.Range(0, 24)
                    .Select(h => new IncidentsByPeriodDto
                    {
                        Label = h.ToString(),
                        Count = counts.TryGetValue(h, out var c) ? c : 0
                    }).ToList();
            }
            else if (period == StatsPeriod.Week)
            {
                var counts = incidents.GroupBy(i => (int)i.CreatedAt.DayOfWeek)
                    .ToDictionary(g => g.Key, g => g.Count());
                string[] days = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
                return Enumerable.Range(0, 7)
                    .Select(d => new IncidentsByPeriodDto
                    {
                        Label = days[d],
                        Count = counts.TryGetValue(d, out var c) ? c : 0
                    }).ToList();
            }
            else
            {
                var counts = incidents.GroupBy(i => i.CreatedAt.Day)
                    .ToDictionary(g => g.Key, g => g.Count());
                return Enumerable.Range(1, 31)
                    .Select(d => new IncidentsByPeriodDto
                    {
                        Label = d.ToString(),
                        Count = counts.TryGetValue(d, out var c) ? c : 0
                    }).ToList();
            }
        }
    }
}