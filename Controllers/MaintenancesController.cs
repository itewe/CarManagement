using CarManagement.Models;
using CarManagement.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CarManagement.Controllers
{
    public class MaintenancesController : Controller
    {
        private readonly MaintenanceRepository _maintenanceRepository;
        private readonly VehicleRepository _vehicleRepository;

        public MaintenancesController(MaintenanceRepository maintenanceRepository, VehicleRepository vehicleRepository)
        {
            _maintenanceRepository = maintenanceRepository;
            _vehicleRepository = vehicleRepository;
        }

        // GET: Maintenances
        //public IActionResult Index()
        //{
        //    var maintenances = _maintenanceRepository.GetAllMaintenances();
        //    return View(maintenances);
        //}
        public IActionResult MaintenancebyVehicle(int id)
        {
            var vehicle = _vehicleRepository.Details(id);
            if (vehicle == null)
            {
                return NotFound();

            }
            var maintenances = _maintenanceRepository.GetAllMaintenancesByVehicleId(id);
            return View("MaitenancebyId", maintenances);
        }

        // GET: Maintenances/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = _maintenanceRepository.GetMaintenanceDetails(id);
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        // GET: Maintenances/Create
        public IActionResult Create()
        {
            var vehicles = _vehicleRepository.GetallVehicles();
            if (vehicles == null)
            {
                return View("Error"); // Handle errors appropriately
            }

            ViewData["VehicleId"] = new SelectList(vehicles, "VehicleId", "PlateNumber");
            return View();
        }

        // POST: Maintenances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Maintenance maintenance)
        {
            if (ModelState.IsValid)
            {
                _maintenanceRepository.CreateMaintenance(maintenance);
                return RedirectToAction(nameof(MaintenancebyVehicle), new { id = maintenance.VehicleId });
            }
            // Repopulate the dropdown in case of errors
            ViewData["VehicleId"] = new SelectList(_vehicleRepository.GetallVehicles(), "VehicleId", "PlateNumber", maintenance.VehicleId);
            return View(maintenance); // Return to the Create view with current model state
        }

        // GET: Maintenances/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = _maintenanceRepository.GetMaintenanceDetails(id);
            if (maintenance == null)
            {
                return NotFound();
            }

            ViewData["VehicleId"] = new SelectList(_vehicleRepository.GetallVehicles(), "VehicleId", "PlateNumber", maintenance.VehicleId);
            return View(maintenance);
        }

        // POST: Maintenances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Maintenance maintenance)
        {
            if (id != maintenance.MaintenanceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _maintenanceRepository.EditMaintenance(id, maintenance);
                return RedirectToAction(nameof(MaintenancebyVehicle), new { id = maintenance.VehicleId });
            }
            //for view bag
            ViewData["VehicleId"] = new SelectList(_vehicleRepository.GetallVehicles(), "VehicleId", "PlateNumber", maintenance.VehicleId);
            return View("MaitenancebyId", maintenance);
        }

        // GET: Maintenances/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = _maintenanceRepository.GetMaintenanceDetails(id);
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        // POST: Maintenances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var maintenance = _maintenanceRepository.GetMaintenanceDetails(id);

            if (ModelState.IsValid)
            {
                _maintenanceRepository.DeleteMaintenance(id);
            }

            return RedirectToAction(nameof(MaintenancebyVehicle), new { id = maintenance.VehicleId });
        }
    }
}
