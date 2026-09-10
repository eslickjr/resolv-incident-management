using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IncidentManagement.Data;
using IncidentManagement.Models;

namespace IncidentManagement.Services
{
    public class IncidentNoteService : IIncidentNoteService
    {
        private readonly IncidentDbContext _db;

        public IncidentNoteService(IncidentDbContext db) => _db = db;

        public async Task<List<IncidentNote>> GetByIncidentAsync(int incidentId) =>
            await _db.IncidentNotes
                .Where(n => n.IncidentId == incidentId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

        public async Task<IncidentNote> AddNoteAsync(int incidentId, string username, string note)
        {
            var incidentNote = new IncidentNote
            {
                IncidentId = incidentId,
                Username   = username,
                Note       = note,
                CreatedAt  = DateTime.UtcNow
            };

            _db.IncidentNotes.Add(incidentNote);
            await _db.SaveChangesAsync();
            return incidentNote;
        }
    }
}