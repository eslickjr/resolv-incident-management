using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IncidentManagement.Services;

namespace IncidentManagement.Controllers
{
    [ApiController]
    [Route("api/incidents/{incidentId}/notes")]
    public class IncidentNotesController : BaseController
    {
        private readonly IIncidentNoteService _noteService;

        public IncidentNotesController(IIncidentNoteService noteService, IConfiguration config)
            : base(config)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotes(int incidentId)
        {
            var notes = await _noteService.GetByIncidentAsync(incidentId);
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(int incidentId, [FromBody] AddNoteRequest request)
        {
            var username = GetUsername();
            if (string.IsNullOrEmpty(username)) return Unauthorized();
            if (string.IsNullOrWhiteSpace(request.Note)) return BadRequest("Note cannot be empty.");

            var note = await _noteService.AddNoteAsync(incidentId, username, request.Note);
            return Ok(note);
        }
    }

    public class AddNoteRequest
    {
        public string Note { get; set; } = string.Empty;
    }
}