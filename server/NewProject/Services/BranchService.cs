using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IncidentManagement.Data;
using IncidentManagement.Models;

namespace IncidentManagement.Services
{
    public interface IBranchService
    {
        Task<List<BranchLocation>> SearchBranchesAsync(string query);
        Task<BranchLocation?> GetByCodeAsync(string branchCode);
    }

    public class BranchService : IBranchService
    {
        private readonly IncidentDbContext _db;

        public BranchService(IncidentDbContext db) => _db = db;

        public async Task<List<BranchLocation>> SearchBranchesAsync(string query)
        {
            var terms = query.Trim().ToLower()
                .Replace(",", " ")
                .Replace(".", " ")
                .Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

            var results = await _db.BranchLocations
                .Where(b => b.IsActive)
                .ToListAsync();

            // Filter in memory for multi-term contains search
            return results.Where(b =>
                terms.All(term =>
                    (b.BranchName?.ToLower().Contains(term) ?? false) ||
                    (b.Address?.ToLower().Contains(term) ?? false) ||
                    (b.City?.ToLower().Contains(term) ?? false) ||
                    (b.State?.ToLower().Contains(term) ?? false) ||
                    (b.ZipCode?.Contains(term) ?? false) ||
                    (b.Phone?.Contains(term) ?? false) ||
                    (b.BranchCode?.Contains(term) ?? false)
                )
            ).Take(20).ToList();
        }

        public async Task<BranchLocation?> GetByCodeAsync(string branchCode) =>
            await _db.BranchLocations
                .Where(b => b.BranchCode == branchCode && b.IsActive)
                .FirstOrDefaultAsync();
    }
}