using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IncidentManagement.Services;

namespace IncidentManagement.Controllers
{
    [ApiController]
    [Route("api/branches")]
    public class BranchesController : BaseController
    {
        private readonly IBranchService _branchService;

        public BranchesController(IBranchService branchService, IConfiguration config)
            : base(config)
        {
            _branchService = branchService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return Ok(new System.Collections.Generic.List<object>());
            var results = await _branchService.SearchBranchesAsync(q);
            return Ok(results);
        }

        [HttpGet("{branchCode}")]
        public async Task<IActionResult> GetByCode(string branchCode)
        {
            var branch = await _branchService.GetByCodeAsync(branchCode);
            if (branch == null) return NotFound();
            return Ok(branch);
        }
    }
}