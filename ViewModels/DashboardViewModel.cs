using System.Collections.Generic;

namespace CarManagement.Models
{
    public class DashboardViewModel
    {
        public int TotalCars { get; set; }
        public int TotalDrivers { get; set; }
        public int TotalMaintenances { get; set; }
        public int ActiveVehicles { get; set; }
        public int InactiveVehicles { get; set; }
        public IEnumerable<Maintenance> LastFiveMaintenances { get; set; }
        public double TotalMaintenanceCost { get; set; }
    }
}
