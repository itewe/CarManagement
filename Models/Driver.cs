using System.ComponentModel.DataAnnotations;

namespace CarManagement.Models
{
    public class Driver
    {
        public int DriverId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = "";

        [StringLength(15)]
        public string? LicenseNumber { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        // Navigation properties
        public List<VehicleDriverHistory> VehicleDriverHistories { get; set; } = new List<VehicleDriverHistory>();
    }
}