using CarManagement.Models;
using CarManagement.Repositories; // Add this if not already included
using CarManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CarManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VehicleRepository _vehicleRepository;
        private readonly DriversRepository _driversRepository;
        private readonly MaintenanceRepository _maintenanceRepository;

        public HomeController(
            ILogger<HomeController> logger,
            VehicleRepository vehicleRepository,
            DriversRepository driversRepository,
            MaintenanceRepository maintenanceRepository)
        {
            _logger = logger;
            _vehicleRepository = vehicleRepository;
            _driversRepository = driversRepository;
            _maintenanceRepository = maintenanceRepository;
        }

        public async Task<IActionResult> Index()
        {
            var totalCars = await _vehicleRepository.GetallVehicles();
            var totalDrivers = await _driversRepository.GetAllDrivers();
            var totalMaintenances = await _maintenanceRepository.GetAllMaintenances();

            var lastFiveMaintenances = await _maintenanceRepository.GetAllMaintenances();

            var totalMaintenanceCost = totalMaintenances.Sum(m => m.Cost);

            var model = new DashboardViewModel
            {
                TotalCars = totalCars.Count(),
                TotalDrivers = totalDrivers.Count(),
                TotalMaintenances = totalMaintenances.Count(),
                ActiveVehicles = totalCars.Count(v => v.CurrentDriverId != null),
                InactiveVehicles = totalCars.Count(v => v.CurrentDriverId == null),
                LastFiveMaintenances = lastFiveMaintenances,
                TotalMaintenanceCost = totalMaintenanceCost
            };

            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
