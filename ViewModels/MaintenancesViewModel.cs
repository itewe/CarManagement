using CarManagement.Models;

namespace CarManagement.ViewModels
{
    public class MaintenancesViewModel
    {
        public IEnumerable<Maintenance> maintenances { get; set; }
        public int VehicleId { get; set; }
    }
}
