using CarManagement.Models;

namespace CarManagement.ViewModels
{
    public class MaintenancesViewModel
    {
        public Maintenance maintenance { get; set; } = new Maintenance();
        public IEnumerable<Maintenance>? maintenances { get; set; }
        public int VehicleId { get; set; }
    }
}
