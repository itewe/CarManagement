using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarManagement.Models
{
    public class VehicleDriverHistory
    {
        public int VehicleDriverHistoryId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [ForeignKey(nameof(VehicleId))]
        public Vehicle? Vehicle { get; set; } // Navigation property

        [Required]
        public int DriverId { get; set; }

        [ForeignKey(nameof(DriverId))] // foreignKey fro the Driver navigation 
        public Driver? Driver { get; set; } // Navigation property

        [Required]
        public DateTime AssignmentDate { get; set; }

        public DateTime? EndDate { get; set; } // Null if the driver is still active
    }
}
