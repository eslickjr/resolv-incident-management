using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IncidentManagement.Data;
using IncidentManagement.Models;

namespace IncidentManagement.Services
{
    public class IssueService : IIssueService
    {
        private readonly IncidentDbContext _db;

        public IssueService(IncidentDbContext db) => _db = db;

        public async Task<List<Issue>> GetActiveIssuesAsync() =>
            await _db.Issues
                .Where(i => i.IsActive)
                .OrderBy(i => i.Name)
                .ToListAsync();

        public async Task<List<Solution>> GetAllActiveSolutionsAsync() =>
            await _db.Solutions
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

        public async Task<List<CallTip>> GetCallTipsForIssueAsync(int issueId) =>
            await _db.CallTips
                .Where(t => t.IssueId == issueId)
                .OrderBy(t => t.SortOrder)
                .ToListAsync();

        public async Task<List<CallTip>> GetAllCallTipsAsync() =>
            await _db.CallTips
                .OrderBy(t => t.IssueId)
                .ThenBy(t => t.SortOrder)
                .ToListAsync();
    }
}