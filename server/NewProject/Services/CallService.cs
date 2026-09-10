using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IncidentManagement.Data;
using IncidentManagement.Models.DTOs;

namespace IncidentManagement.Services
{
    public class CallService : ICallService
    {
        private readonly IncidentDbContext _db;
        private readonly ILoanService _loanService;
        private readonly IIncidentService _incidentService;

        public CallService(IncidentDbContext db, ILoanService loanService, IIncidentService incidentService)
        {
            _db = db;
            _loanService = loanService;
            _incidentService = incidentService;
        }

        private static string NormalizePhone(string phone) =>
            new string(phone.Where(char.IsDigit).ToArray());

        private static string NormalizeName(string name) =>
            name.Trim().ToUpperInvariant();

        private static string NormalizeSSN(string? ssn) =>
            ssn?.Replace("-", "").Replace(" ", "") ?? "";

        public async Task<CallMatchResult> ProcessCallAsync(string phone, string firstName, string lastName)
        {
            var normalizedPhone = NormalizePhone(phone);
            var normalizedFirst = NormalizeName(firstName);
            var normalizedLast  = NormalizeName(lastName);

            // ── Step 1: Exact phone + name match in incidents ─────────────
            var incidentExact = await _db.Incidents
                .Where(i =>
                    i.Phone == normalizedPhone &&
                    i.FirstName.ToUpper() == normalizedFirst &&
                    i.LastName.ToUpper()  == normalizedLast)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            if (incidentExact.Any())
            {
                var open   = incidentExact.FirstOrDefault(i => i.ClosedAt == null);
                var target = open ?? incidentExact.First();
                return new CallMatchResult
                {
                    MatchType  = "incident_exact",
                    IncidentId = open?.IncidentId,
                    Branch     = target.Branch,
                    Account    = target.Account,
                    Phone      = phone,
                    FirstName  = target.FirstName,
                    LastName   = target.LastName
                };
            }

            // ── Step 2: Exact phone + name match in loans ─────────────────
            var loans = await _loanService.GetLoansByPhoneAsync(normalizedPhone);
            var loanExact = loans
                .Where(l =>
                    NormalizeName(l.FirstName) == normalizedFirst &&
                    NormalizeName(l.LastName)  == normalizedLast)
                .ToList();

            if (loanExact.Any())
            {
                var loan   = loanExact.First();
                var rawSSN = NormalizeSSN(loan.TaxId);

                if (!string.IsNullOrEmpty(rawSSN))
                {
                    var incidentBySSN = await _db.Incidents
                        .Where(i => i.SSN == rawSSN)
                        .OrderByDescending(i => i.CreatedAt)
                        .ToListAsync();

                    if (incidentBySSN.Any())
                    {
                        var open = incidentBySSN.FirstOrDefault(i => i.ClosedAt == null);
                        return new CallMatchResult
                        {
                            MatchType  = "loan_exact",
                            IncidentId = open?.IncidentId,
                            Branch     = loan.BranchCode,
                            Account    = loan.AccountNumber,
                            Phone      = phone,
                            FirstName  = loan.FirstName,
                            LastName   = loan.LastName
                        };
                    }
                }

                return new CallMatchResult
                {
                    MatchType = "loan_exact_new",
                    Branch    = loan.BranchCode,
                    Account   = loan.AccountNumber,
                    Phone     = phone,
                    FirstName = loan.FirstName,
                    LastName  = loan.LastName
                };
            }

            // ── Step 3 & 4: Phone-only matches ───────────────────────────
            var incidentPhoneMatches = await _db.Incidents
                .Where(i => i.Phone == normalizedPhone)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            var deduped = incidentPhoneMatches
                .GroupBy(i => i.SSN)
                .Select(g => g.First())
                .ToList();

            var loanPhoneMatches = loans
                .GroupBy(l => l.TaxId)
                .Select(g => g.First())
                .ToList();

            if (deduped.Any() || loanPhoneMatches.Any())
            {
                var incidentSSNs = deduped
                    .Where(i => i.SSN != null)
                    .Select(i => NormalizeSSN(i.SSN))
                    .ToHashSet();

                var matches = new List<CallMatchItem>();

                matches.AddRange(deduped.Select(i =>
                {
                    var matchingLoan = loanPhoneMatches
                        .FirstOrDefault(l => NormalizeSSN(l.TaxId) == NormalizeSSN(i.SSN));
                    return new CallMatchItem
                    {
                        Source     = "incident",
                        IncidentId = i.IncidentId,
                        Branch     = matchingLoan?.BranchCode ?? i.Branch,
                        Account    = matchingLoan?.AccountNumber ?? i.Account,
                        FullName   = $"{i.FirstName} {i.LastName}",
                        Phone      = phone,
                        Issue      = i.Issue,
                        ClosedAt   = i.ClosedAt?.ToString("o"),
                        HasLoan    = matchingLoan != null
                    };
                }));

                matches.AddRange(loanPhoneMatches
                    .Where(l => !incidentSSNs.Contains(NormalizeSSN(l.TaxId)))
                    .Select(l => new CallMatchItem
                    {
                        Source   = "loan",
                        Branch   = l.BranchCode,
                        Account  = l.AccountNumber,
                        FullName = $"{l.FirstName} {l.LastName}",
                        Phone    = phone,
                        SSN      = l.TaxId
                    }));

                return new CallMatchResult
                {
                    MatchType = "phone_only",
                    Phone     = phone,
                    FirstName = firstName,
                    LastName  = lastName,
                    Matches   = matches
                };
            }

            return new CallMatchResult
            {
                MatchType = "no_match",
                Phone     = phone,
                FirstName = firstName,
                LastName  = lastName
            };
        }
    }
}