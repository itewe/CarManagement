using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarManagement.Models
{
    public class VehicleDriverHistory
    {
        public int VehicleDriverHistoryId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        required

        public Vehicle Vehicle
        { get; set; }

        [Required]
        public int DriverId { get; set; }

        [ForeignKey("DriverId")]
        required

        public Driver Driver
        { get; set; }

        [Required]
        public DateTime AssignmentDate { get; set; }

        public DateTime? EndDate { get; set; } // Null if the driver is still active
    }
}
