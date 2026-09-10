using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IncidentManagement.Services;

namespace IncidentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : BaseController
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService, IConfiguration config)
            : base(config)
        {
            _loanService = loanService;
        }

        [HttpGet("{branch}/{account}")]
        public async Task<IActionResult> GetLoanInformation(string branch, string account)
        {
            var loan = await _loanService.GetLoanAccountAsync(branch, account);
            if (loan is null)
                return NotFound($"No loan found for Branch: {branch}, Account: {account}.");

            return Ok(loan);
        }

        [HttpGet("{branch}/{account}/payments")]
        public async Task<IActionResult> GetPaymentHistory(string branch, string account)
        {
            var loanReference = $"{branch}-{account}";
            var payments = await _loanService.GetPaymentHistoryAsync(loanReference);
            return Ok(payments);
        }

        [HttpGet("history/{taxId}")]
        public async Task<IActionResult> GetLoanHistory(string taxId)
        {
            if (string.IsNullOrWhiteSpace(taxId))
                return BadRequest("Tax ID is required.");

            var history = await _loanService.GetLoanHistoryByTaxIdAsync(taxId);
            return Ok(history);
        }
    }
}