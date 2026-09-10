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
    public class IncidentService : IIncidentService
    {
        private readonly IncidentDbContext _db;

        public IncidentService(IncidentDbContext db)
        {
            _db = db;
        }

        public async Task<Incident> CreateAsync(CreateIncidentRequest request, string createdBy)
        {
            var incident = new Incident
            {
                FirstName = request.FirstName,
                LastName  = request.LastName,
                SSN       = request.SSN   != null ? NormalizeSSN(request.SSN)       : null,
                Phone     = request.Phone != null ? NormalizePhone(request.Phone)   : null,
                Branch    = request.Branch != null ? NormalizeBranch(request.Branch) : string.Empty,
                Account   = request.Account,
                Issue     = string.Empty,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow
            };

            _db.Incidents.Add(incident);
            await _db.SaveChangesAsync();

            _db.IncidentStatTrackings.Add(new IncidentStatTracking
            {
                IncidentId       = incident.IncidentId,
                Username         = createdBy,
                Opened           = true,
                TimeSpentSeconds = 0,
                Closed           = false
            });

            await _db.SaveChangesAsync();
            return incident;
        }

        public async Task<Incident?> PatchAsync(
            int incidentId, UpdateIncidentRequest request, string username)
        {
            var incident = await _db.Incidents.FindAsync(incidentId);
            if (incident is null) return null;

            if (request.Branch            != null) incident.Branch            = NormalizeBranch(request.Branch);
            if (request.Phone             != null) incident.Phone             = NormalizePhone(request.Phone);
            if (request.SSN               != null) incident.SSN               = NormalizeSSN(request.SSN);
            if (request.FirstName         != null) incident.FirstName         = request.FirstName;
            if (request.LastName          != null) incident.LastName          = request.LastName;
            if (request.Issue             != null) incident.Issue             = request.Issue;
            if (request.Solution          != null) incident.Solution          = request.Solution;
            if (request.AdditionalDetails != null) incident.AdditionalDetails = request.AdditionalDetails;

            if (request.IsClosing && incident.ClosedAt == null)
            {
                incident.ClosedBy = username;
                incident.ClosedAt = DateTime.UtcNow;
            }

            var existing = await _db.IncidentStatTrackings
                .FirstOrDefaultAsync(s => s.IncidentId == incidentId && s.Username == username);

            if (existing != null)
            {
                existing.TimeSpentSeconds += request.TimeSpentSeconds;
                if (request.IsClosing) existing.Closed = true;
            }
            else
            {
                _db.IncidentStatTrackings.Add(new IncidentStatTracking
                {
                    IncidentId       = incidentId,
                    Username         = username,
                    Opened           = false,
                    TimeSpentSeconds = request.TimeSpentSeconds,
                    Closed           = request.IsClosing
                });
            }

            await _db.SaveChangesAsync();
            return incident;
        }

        public async Task<Incident?> GetByIdAsync(int incidentId)
        {
            return await _db.Incidents.FindAsync(incidentId);
        }

        public async Task<List<IncidentHistory>> GetByCustomerSSNAsync(string ssn)
        {
            return await _db.IncidentHistories
                .FromSqlRaw("SELECT * FROM vw_IncidentHistory WHERE SSN = {0}", ssn)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Incident>> GetRecentByUserAsync(string username)
        {
            var incidentIds = await _db.IncidentStatTrackings
                .Where(s => s.Username == username)
                .Select(s => s.IncidentId)
                .Distinct()
                .ToListAsync();

            return await _db.Incidents
                .Where(i => incidentIds.Contains(i.IncidentId))
                .OrderByDescending(i => i.CreatedAt)
                .Take(10)
                .ToListAsync();
        }

        public async Task<Incident?> GetOpenIncidentBySSNAsync(string ssn)
        {
            return await _db.Incidents
                .Where(i => i.SSN == ssn && i.ClosedAt == null)
                .OrderByDescending(i => i.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateTimeAsync(int incidentId, string username, int timeSpentSeconds)
        {
            var stat = await _db.IncidentStatTrackings
                .FirstOrDefaultAsync(s => s.IncidentId == incidentId && s.Username == username);

            if (stat != null)
            {
                stat.TimeSpentSeconds += timeSpentSeconds;
                await _db.SaveChangesAsync();
            }
        }

        private static string NormalizeBranch(string branch)
        {
            var digits = new string(branch.Where(char.IsDigit).ToArray());
            return digits.PadLeft(4, '0');
        }

        private static string NormalizePhone(string phone)
        {
            return new string(phone.Where(char.IsDigit).ToArray());
        }

        private static string NormalizeSSN(string ssn)
        {
            return new string(ssn.Where(char.IsDigit).ToArray());
        }
    }
}