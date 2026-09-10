using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IncidentManagement.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly IConfiguration _config;

        public BaseController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Gets the current username from the Azure AD token,
        /// or falls back to DevUsername in appsettings when BypassAuth is true.
        /// </summary>
        protected string? GetUsername()
        {
            if (_config.GetValue<bool>("BypassAuth"))
                return _config.GetValue<string>("DevUsername") ?? "devuser";

            return User.FindFirst("preferred_username")?.Value
                ?? User.FindFirst("upn")?.Value
                ?? User.Identity?.Name;
        }
    }
}