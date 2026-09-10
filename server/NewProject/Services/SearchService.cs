using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IncidentManagement.Data;
using IncidentManagement.Models;
using IncidentManagement.Models.DTOs;

namespace IncidentManagement.Services
{
    public interface ISearchService
    {
        Task<List<SearchResultDto>> SearchBySSNAsync(string ssn);
        Task<List<SearchResultDto>> SearchByNameAsync(string firstName, string lastName);
    }

    public class SearchService : ISearchService
    {
        private readonly IncidentDbContext _db;

        public SearchService(IncidentDbContext db) => _db = db;

        public async Task<List<SearchResultDto>> SearchBySSNAsync(string ssn)
        {
            var results = new List<SearchResultDto>();
            var normalizedSSN = ssn.Replace("-", "").Replace(" ", "");

            var incidents = await _db.Incidents
                .Where(i => i.SSN == normalizedSSN)
                .OrderByDescending(i => i.CreatedAt)
                .Take(10)
                .ToListAsync();

            var loans = await _db.LoanAccounts
                .Where(l => l.TaxId == normalizedSSN)
                .OrderByDescending(l => l.OriginationDate)
                .Take(10)
                .ToListAsync();

            var loanTaxIds = loans.Select(l => l.TaxId).ToHashSet();

            results.AddRange(incidents.Select(i => new SearchResultDto
            {
                IncidentId = i.IncidentId,
                FullName   = $"{i.FirstName} {i.LastName}",
                FirstName  = i.FirstName,
                LastName   = i.LastName,
                SSN        = i.SSN,
                Phone      = i.Phone,
                Branch     = i.Branch,
                Account    = i.Account,
                ClosedAt   = i.ClosedAt,
                Priority   = loanTaxIds.Contains(i.SSN) ? 1 : 2
            }));

            results.AddRange(loans
                .Where(l => !results.Any(r => r.SSN == l.TaxId))
                .Select(l => new SearchResultDto
                {
                    LoanId    = l.LoanId,
                    FullName  = $"{l.FirstName} {l.LastName}",
                    FirstName = l.FirstName,
                    LastName  = l.LastName,
                    SSN       = l.TaxId,
                    Phone     = l.MobilePhone ?? l.HomePhone,
                    Branch    = l.BranchCode,
                    Account   = l.AccountNumber,
                    Priority  = 3
                }));

            return results.OrderBy(r => r.Priority).ToList();
        }

        public async Task<List<SearchResultDto>> SearchByNameAsync(string firstName, string lastName)
        {
            var results = new List<SearchResultDto>();
            bool singleTerm = string.IsNullOrWhiteSpace(firstName) ||
                            string.IsNullOrWhiteSpace(lastName) ||
                            firstName == lastName;

            if (singleTerm)
            {
                var term = (string.IsNullOrWhiteSpace(firstName) ? lastName : firstName).ToUpper();

                var incidents = await _db.Incidents
                    .Where(i => i.FirstName.ToUpper().Contains(term) ||
                                i.LastName.ToUpper().Contains(term))
                    .OrderByDescending(i => i.CreatedAt)
                    .Take(10)
                    .ToListAsync();

                var loans = await _db.LoanAccounts
                    .Where(l => l.FirstName.ToUpper().Contains(term) ||
                                l.LastName.ToUpper().Contains(term))
                    .OrderByDescending(l => l.OriginationDate)
                    .Take(10)
                    .ToListAsync();

                var loanTaxIds = loans.Select(l => l.TaxId).ToHashSet();

                results.AddRange(incidents.Select(i => new SearchResultDto
                {
                    IncidentId = i.IncidentId,
                    FullName   = $"{i.FirstName} {i.LastName}",
                    FirstName  = i.FirstName,
                    LastName   = i.LastName,
                    SSN        = i.SSN,
                    Phone      = i.Phone,
                    Branch     = i.Branch,
                    Account    = i.Account,
                    ClosedAt   = i.ClosedAt,
                    Priority   = loanTaxIds.Contains(i.SSN) ? 1 : 2
                }));

                results.AddRange(loans
                    .Where(l => !results.Any(r => r.SSN == l.TaxId))
                    .Select(l => new SearchResultDto
                    {
                        LoanId    = l.LoanId,
                        FullName  = $"{l.FirstName} {l.LastName}",
                        FirstName = l.FirstName,
                        LastName  = l.LastName,
                        SSN       = l.TaxId,
                        Phone     = l.MobilePhone ?? l.HomePhone,
                        Branch    = l.BranchCode,
                        Account   = l.AccountNumber,
                        Priority  = 3
                    }));

                return results.OrderBy(r => r.Priority).Take(10).ToList();
            }

            // ── Two-term search ───────────────────────────────────────────────────

            // Exact match on incidents
            var exactIncidents = await _db.Incidents
                .Where(i => i.FirstName.ToUpper() == firstName.ToUpper() &&
                            i.LastName.ToUpper()  == lastName.ToUpper())
                .OrderByDescending(i => i.CreatedAt)
                .Take(10)
                .ToListAsync();

            // Exact match on loans
            var exactLoans = await _db.LoanAccounts
                .Where(l => l.FirstName.ToUpper() == firstName.ToUpper() &&
                            l.LastName.ToUpper()  == lastName.ToUpper())
                .OrderByDescending(l => l.OriginationDate)
                .Take(10)
                .ToListAsync();

            var exactLoanTaxIds = exactLoans.Select(l => l.TaxId).ToHashSet();

            results.AddRange(exactIncidents.Select(i => new SearchResultDto
            {
                IncidentId = i.IncidentId,
                FullName   = $"{i.FirstName} {i.LastName}",
                FirstName  = i.FirstName,
                LastName   = i.LastName,
                SSN        = i.SSN,
                Phone      = i.Phone,
                Branch     = i.Branch,
                Account    = i.Account,
                ClosedAt   = i.ClosedAt,
                Priority   = exactLoanTaxIds.Contains(i.SSN) ? 1 : 2
            }));

            results.AddRange(exactLoans
                .Where(l => !results.Any(r => r.SSN == l.TaxId))
                .Select(l => new SearchResultDto
                {
                    LoanId    = l.LoanId,
                    FullName  = $"{l.FirstName} {l.LastName}",
                    FirstName = l.FirstName,
                    LastName  = l.LastName,
                    SSN       = l.TaxId,
                    Phone     = l.MobilePhone ?? l.HomePhone,
                    Branch    = l.BranchCode,
                    Account   = l.AccountNumber,
                    Priority  = 3
                }));

            if (results.Count >= 10) return results.OrderBy(r => r.Priority).ToList();

            // Loose match on incidents
            var looseIncidents = await _db.Incidents
                .Where(i => i.FirstName.ToUpper().Contains(firstName.ToUpper()) &&
                            i.LastName.ToUpper().Contains(lastName.ToUpper()))
                .OrderByDescending(i => i.CreatedAt)
                .Take(10)
                .ToListAsync();

            // Loose match on loans
            var looseLoans = await _db.LoanAccounts
                .Where(l => l.FirstName.ToUpper().Contains(firstName.ToUpper()) &&
                            l.LastName.ToUpper().Contains(lastName.ToUpper()))
                .OrderByDescending(l => l.OriginationDate)
                .Take(10)
                .ToListAsync();

            var looseLoanTaxIds = looseLoans.Select(l => l.TaxId).ToHashSet();

            results.AddRange(looseIncidents
                .Where(i => !results.Any(r => r.IncidentId == i.IncidentId))
                .Select(i => new SearchResultDto
                {
                    IncidentId = i.IncidentId,
                    FullName   = $"{i.FirstName} {i.LastName}",
                    FirstName  = i.FirstName,
                    LastName   = i.LastName,
                    SSN        = i.SSN,
                    Phone      = i.Phone,
                    Branch     = i.Branch,
                    Account    = i.Account,
                    ClosedAt   = i.ClosedAt,
                    Priority   = looseLoanTaxIds.Contains(i.SSN) ? 1 : 2
                }));

            results.AddRange(looseLoans
                .Where(l => !results.Any(r => r.SSN == l.TaxId))
                .Select(l => new SearchResultDto
                {
                    LoanId    = l.LoanId,
                    FullName  = $"{l.FirstName} {l.LastName}",
                    FirstName = l.FirstName,
                    LastName  = l.LastName,
                    SSN       = l.TaxId,
                    Phone     = l.MobilePhone ?? l.HomePhone,
                    Branch    = l.BranchCode,
                    Account   = l.AccountNumber,
                    Priority  = 3
                }));

            return results.OrderBy(r => r.Priority).Take(10).ToList();
        }
    }
}