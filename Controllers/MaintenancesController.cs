using CarManagement.Models;
using CarManagement.Repositories;
using CarManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> Index()
        {
            var maintenances = await _maintenanceRepository.GetAllMaintenances();
            return View(maintenances);
        }

        public async Task<IActionResult> MaintenancebyVehicle(int id)
        {
            var vehicle = await _vehicleRepository.Details(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            var maintenances = await _maintenanceRepository.GetAllMaintenancesByVehicleId(id);
            var viewModel = new MaintenancesViewModel
            {
                VehicleId = id,
                maintenances = maintenances
            };
            ViewData["Vehiclepltnb"] = vehicle.PlateNumber;
            return View("MaintenancebyId", viewModel);
        }

        // GET: Maintenances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = await _maintenanceRepository.GetMaintenanceDetails(id);
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        // GET: Maintenances/Create/1
        public async Task<IActionResult> Create(int id)
        {
            var vehicle = await _vehicleRepository.Details(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            ViewBag.Vehicles = new SelectList(await _vehicleRepository.GetallVehicles(), "VehicleId", "PlateNumber");
            var model = new MaintenancesViewModel
            {
                VehicleId = id,
                maintenance = new Maintenance(),
                maintenances = new List<Maintenance>()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenancesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var vehicle = await _vehicleRepository.Details(model.VehicleId);
                if (vehicle == null)
                {
                    return NotFound();
                }

                var maintenance = new Maintenance
                {
                    Description = model.maintenance.Description,
                    DateOfMaintenance = model.maintenance.DateOfMaintenance,
                    Cost = model.maintenance.Cost,
                    VehicleId = model.VehicleId
                };

                await _maintenanceRepository.CreateMaintenance(maintenance);

                return RedirectToAction(nameof(MaintenancebyVehicle), new { id = maintenance.VehicleId });
            }

            return View(model);
        }

        // GET: Maintenances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = await _maintenanceRepository.GetMaintenanceDetails(id);
            if (maintenance == null)
            {
                return NotFound();
            }

            ViewData["VehicleId"] = new SelectList(await _vehicleRepository.GetallVehicles(), "VehicleId", "PlateNumber", maintenance.VehicleId);
            return View(maintenance);
        }

        // POST: Maintenances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Maintenance maintenance)
        {
            if (id != maintenance.MaintenanceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _maintenanceRepository.EditMaintenance(id, maintenance);
                return RedirectToAction(nameof(MaintenancebyVehicle), new { id = maintenance.VehicleId });
            }

            ViewData["VehicleId"] = new SelectList(await _vehicleRepository.GetallVehicles(), "VehicleId", "PlateNumber", maintenance.VehicleId);
            return View("MaintenancebyId", maintenance);
        }

        // GET: Maintenances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = await _maintenanceRepository.GetMaintenanceDetails(id);
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        // POST: Maintenances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maintenance = await _maintenanceRepository.GetMaintenanceDetails(id);

            if (ModelState.IsValid)
            {
                await _maintenanceRepository.DeleteMaintenance(id);
            }

            return RedirectToAction(nameof(MaintenancebyVehicle), new { id = maintenance.VehicleId });
        }
    }
}
