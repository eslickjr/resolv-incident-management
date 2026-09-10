using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IncidentManagement.Data;
using IncidentManagement.Models;
using IncidentManagement.Models.DTOs;

namespace IncidentManagement.Services
{
    public interface ILoanService
    {
        Task<LoanAccount?> GetLoanAccountAsync(string branchCode, string accountNumber);
        Task<List<PaymentTransaction>> GetPaymentHistoryAsync(string loanReference);
        Task<List<LoanAccount>> GetLoanHistoryByTaxIdAsync(string taxId);
        Task<List<LoanAccount>> GetLoansByPhoneAsync(string phone);
        Task<List<SearchResultDto>> SearchByTaxIdAsync(string taxId);
    }

    public class LoanService : ILoanService
    {
        private readonly IncidentDbContext _db;

        public LoanService(IncidentDbContext db) => _db = db;

        public async Task<LoanAccount?> GetLoanAccountAsync(string branchCode, string accountNumber) =>
            await _db.LoanAccounts
                .Where(l => l.BranchCode == branchCode && l.AccountNumber == accountNumber)
                .FirstOrDefaultAsync();

        public async Task<List<PaymentTransaction>> GetPaymentHistoryAsync(string loanReference) =>
            await _db.PaymentTransactions
                .Where(p => p.LoanReference == loanReference)
                .OrderByDescending(p => p.TransactionDate)
                .ToListAsync();

        public async Task<List<LoanAccount>> GetLoanHistoryByTaxIdAsync(string taxId) =>
            await _db.LoanAccounts
                .Where(l => l.TaxId == taxId)
                .OrderByDescending(l => l.OriginationDate)
                .ToListAsync();

        public async Task<List<LoanAccount>> GetLoansByPhoneAsync(string phone)
        {
            var digits = new string(phone.Where(char.IsDigit).ToArray());
            return await _db.LoanAccounts
                .Where(l =>
                    (l.MobilePhone != null && l.MobilePhone.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "") == digits) ||
                    (l.HomePhone  != null && l.HomePhone.Replace("-",  "").Replace(" ", "").Replace("(", "").Replace(")", "") == digits))
                .OrderByDescending(l => l.OriginationDate)
                .ToListAsync();
        }

        public async Task<List<SearchResultDto>> SearchByTaxIdAsync(string taxId)
        {
            var loans = await _db.LoanAccounts
                .Where(l => l.TaxId == taxId)
                .OrderByDescending(l => l.OriginationDate)
                .ToListAsync();

            return loans.Select(l => new SearchResultDto
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
            }).ToList();
        }
    }
}