using Microsoft.EntityFrameworkCore;

namespace IncidentManagement.Models
{
    [Keyless]
    public class Branch
    {
        public int LocationId { get; set; }
        public string? LocationCode { get; set; }
        public string? LocationName { get; set; }
        public string? PhysicalAddress1 { get; set; }
        public string? PhysicalCity { get; set; }
        public string? PhysicalState { get; set; }
        public string? PhysicalZip { get; set; }
        public string? PhoneMain { get; set; }
        public bool IsActive { get; set; }
    }
}