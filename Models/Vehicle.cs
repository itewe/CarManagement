using System.ComponentModel.DataAnnotations;

namespace CarManagement.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }

        [Required]
        [StringLength(10)]
        public string PlateNumber { get; set; } = "";

        [Range(1, 100)]
        public int NumberOfSeats { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(50)]
        public string? Type { get; set; }

        // Navigation properties one-to-many
        public List<VehicleDriverHistory> VehicleDriverHistories { get; set; } = new List<VehicleDriverHistory>();

        public List<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
    }
}