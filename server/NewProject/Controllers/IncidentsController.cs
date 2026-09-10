using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IncidentManagement.Models.DTOs;
using IncidentManagement.Services;

namespace IncidentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentsController : BaseController
    {
        private readonly IIncidentService _incidentService;

        public IncidentsController(IIncidentService incidentService, IConfiguration config)
            : base(config)
        {
            _incidentService = incidentService;
        }

        [HttpGet("{incidentId:int}")]
        public async Task<IActionResult> GetById(int incidentId)
        {
            var incident = await _incidentService.GetByIdAsync(incidentId);
            if (incident is null) return NotFound();
            return Ok(incident);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncident([FromBody] CreateIncidentRequest request)
        {
            var username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Could not determine user.");

            var incident = await _incidentService.CreateAsync(request, username);
            return CreatedAtAction(nameof(GetIncidentsByCustomer),
                new { ssn = incident.SSN }, incident);
        }

        [HttpPatch("{incidentId:int}")]
        public async Task<IActionResult> UpdateIncident(
            int incidentId, [FromBody] UpdateIncidentRequest request)
        {
            var username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Could not determine user.");

            var updated = await _incidentService.PatchAsync(incidentId, request, username);
            if (updated is null)
                return NotFound($"Incident {incidentId} not found.");

            return Ok(updated);
        }

        [HttpGet("customer/{ssn}")]
        public async Task<IActionResult> GetIncidentsByCustomer(string ssn)
        {
            if (string.IsNullOrWhiteSpace(ssn))
                return BadRequest("SSN is required.");

            var history = await _incidentService.GetByCustomerSSNAsync(ssn);
            return Ok(history);
        }

        [HttpGet("open/{ssn}")]
        public async Task<IActionResult> GetOpenIncidentBySSN(string ssn)
        {
            var incident = await _incidentService.GetOpenIncidentBySSNAsync(ssn);
            if (incident is null) return NotFound();
            return Ok(incident);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentByUser()
        {
            var username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Could not determine user.");

            var incidents = await _incidentService.GetRecentByUserAsync(username);
            return Ok(incidents);
        }

        [HttpPost("{id}/beacon")]
        [Consumes("application/json")]
        public async Task<IActionResult> Beacon(int id, [FromBody] BeaconRequest request)
        {
            var username = GetUsername();
            if (username == null) return Unauthorized();
            await _incidentService.UpdateTimeAsync(id, username, request.TimeSpentSeconds);
            return Ok();
        }
    }
}