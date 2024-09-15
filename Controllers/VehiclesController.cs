using Microsoft.AspNetCore.Mvc;
using CarManagement.Data;
using CarManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarManagement.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly VehicleRepository vehicleRepository;
        private readonly DriversRepository driversRepository;

        public VehiclesController(VehicleRepository vehicleRepository, DriversRepository driversRepository)
        {
            this.vehicleRepository = vehicleRepository;
            this.driversRepository = driversRepository;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var vehicles = await vehicleRepository.GetallVehicles();
            return View(vehicles);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await vehicleRepository.Details(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await vehicleRepository.Create(vehicle);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Show the error message to the user
                    ModelState.AddModelError(string.Empty, "A vehicle with the same Plate Number already exists.");
                }
            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await vehicleRepository.Details(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle)
        {
            if (id != vehicle.VehicleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await vehicleRepository.Edit(id, vehicle);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Show the error message to the user
                    ModelState.AddModelError(string.Empty, "A vehicle with the same Plate Number already exists.");
                }
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await vehicleRepository.Details(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await vehicleRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Vehicles/EditDriver/5
        public async Task<IActionResult> EditDriver(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await vehicleRepository.Details(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            var drivers = await driversRepository.GetAllDrivers();

            var viewModel = new EditDriverViewModel
            {
                VehicleId = vehicle.VehicleId,
                SelectedDriverId = vehicle.CurrentDriverId.HasValue ? vehicle.CurrentDriverId.Value : 0,
                Drivers = new SelectList(drivers, "DriverId", "LicenseNumber")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDriver(EditDriverViewModel model)
        {
            try
            {
                await vehicleRepository.UpdateDriver(model.VehicleId, model.SelectedDriverId);
                return Redirect($"/VehicleDriverHistories/HistorybyvehicleId/{model.VehicleId}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the driver.");
            }

            var drivers = await driversRepository.GetAllDrivers();
            model.Drivers = new SelectList(drivers, "DriverId", "LicenseNumber");
            return View(model);
        }
    }
}
