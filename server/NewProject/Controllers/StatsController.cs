using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IncidentManagement.Services;

namespace IncidentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : BaseController
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService statsService, IConfiguration config)
            : base(config)
        {
            _statsService = statsService;
        }

        [HttpGet("aggregate/{period}")]
        public async Task<IActionResult> GetAggregateStats(string period)
        {
            var username = GetUsername();
            if (string.IsNullOrEmpty(username)) return Unauthorized();
            if (!await _statsService.IsAdminAsync(username)) return Forbid();

            if (!TryParsePeriod(period, out var statsPeriod))
                return BadRequest("Period must be 'day', 'week', or 'month'.");

            var stats = await _statsService.GetAggregateStatsAsync(statsPeriod);
            return Ok(stats);
}

        [HttpGet("{period}")]
        public async Task<IActionResult> GetMyStats(
            string period,
            [FromQuery] string? filterUsername = null)
        {
            var username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Could not determine user.");

            if (!TryParsePeriod(period, out var statsPeriod))
                return BadRequest("Period must be 'day', 'week', or 'month'.");

            var stats = await _statsService.GetStatsAsync(username, statsPeriod, filterUsername);
            return Ok(stats);
        }

        [HttpGet("all/{period}")]
        public async Task<IActionResult> GetAllStats(string period)
        {
            var username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            if (!await _statsService.IsAdminAsync(username))
                return Forbid();

            if (!TryParsePeriod(period, out var statsPeriod))
                return BadRequest("Period must be 'day', 'week', or 'month'.");

            var stats = await _statsService.GetAllUserStatsAsync(statsPeriod);
            return Ok(stats);
        }

        private static bool TryParsePeriod(string period, out StatsPeriod result)
        {
            result = period.ToLower() switch
            {
                "day"   => StatsPeriod.Day,
                "week"  => StatsPeriod.Week,
                "month" => StatsPeriod.Month,
                _       => StatsPeriod.Month
            };

            var p = period.ToLower();
            return p == "day" || p == "week" || p == "month";
        }
    }
}