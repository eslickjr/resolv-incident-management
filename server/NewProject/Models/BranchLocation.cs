using System.ComponentModel.DataAnnotations;

namespace IncidentManagement.Models
{
    public class BranchLocation
    {
        [Key]
        public int BranchId { get; set; }

        [StringLength(10)]
        public string BranchCode { get; set; } = string.Empty;

        [StringLength(100)]
        public string BranchName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(2)]
        public string? State { get; set; }

        [StringLength(10)]
        public string? ZipCode { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        public bool IsActive { get; set; } = true;
    }
}