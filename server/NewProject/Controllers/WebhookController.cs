using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using IncidentManagement.Hubs;
using IncidentManagement.Models.DTOs;
using IncidentManagement.Services;

namespace IncidentManagement.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class WebhookController : BaseController
    {
        private readonly ICallService _callService;
        private readonly IHubContext<CallHub> _hub;

        public WebhookController(
            ICallService callService,
            IHubContext<CallHub> hub,
            IConfiguration config) : base(config)
        {
            _callService = callService;
            _hub = hub;
        }

        [HttpPost("call")]
        public async Task<IActionResult> IncomingCall([FromBody] WebhookCallRequest request)
        {
            var result = await _callService.ProcessCallAsync(
                request.Phone, request.FirstName, request.LastName);

            // Broadcast to all connected frontend clients
            await _hub.Clients.All.SendAsync("IncomingCall", result);

            return Ok(result);
        }
    }
}