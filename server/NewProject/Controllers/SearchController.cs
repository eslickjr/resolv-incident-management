using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IncidentManagement.Services;

namespace IncidentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : BaseController
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService, IConfiguration config)
            : base(config)
        {
            _searchService = searchService;
        }

        [HttpGet("ssn/{ssn}")]
        public async Task<IActionResult> SearchBySSN(string ssn)
        {
            if (string.IsNullOrWhiteSpace(ssn))
                return BadRequest("SSN is required.");

            var results = await _searchService.SearchBySSNAsync(ssn);
            return Ok(results);
        }

        [HttpGet("name/{firstName}")]
        [HttpGet("name/{firstName}/{lastName}")]
        public async Task<IActionResult> SearchByName(string firstName, string? lastName = null)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return BadRequest("Name is required.");

            var results = await _searchService.SearchByNameAsync(firstName, lastName ?? "");
            return Ok(results);
        }
    }
}