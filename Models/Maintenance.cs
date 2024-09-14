using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarManagement.Models
{
    public class Maintenance
    {
        public int MaintenanceId { get; set; }

        [Required]
        public string Description { get; set; } = "";

        [Required]
        public DateTime DateOfMaintenance { get; set; }

        [Range(0, double.MaxValue)]
        public double Cost { get; set; }

        // Foreign key to Vehicle
        [Required]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle? Vehicle { get; set; }
    }
}
