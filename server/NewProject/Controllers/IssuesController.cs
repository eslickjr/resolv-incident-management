using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IncidentManagement.Services;

namespace IncidentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : BaseController
    {
        private readonly IIssueService _issueService;

        public IssuesController(IIssueService issueService, IConfiguration config)
            : base(config)
        {
            _issueService = issueService;
        }

        [HttpGet]
        public async Task<IActionResult> GetIssues()
        {
            var issues = await _issueService.GetActiveIssuesAsync();
            return Ok(issues.Select(i => new { i.IssueId, i.Name }));
        }

        [HttpGet("solutions")]
        public async Task<IActionResult> GetSolutions()
        {
            var solutions = await _issueService.GetAllActiveSolutionsAsync();
            return Ok(solutions.Select(s => new { s.SolutionId, s.IssueId, s.Name }));
        }

        [HttpGet("tips")]
        public async Task<IActionResult> GetAllTips()
        {
            var tips = await _issueService.GetAllCallTipsAsync();
            return Ok(tips.Select(t => new { t.TipId, t.IssueId, t.Tip, t.SortOrder }));
        }
    }
}