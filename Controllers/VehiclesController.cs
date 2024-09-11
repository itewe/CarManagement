using Microsoft.AspNetCore.Mvc;
using CarManagement.Data;
using CarManagement.Models;

namespace CarManagement.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly VehicleRepository vehicleRepository;

        public VehiclesController(VehicleRepository vehicleRepository)
        {
            this.vehicleRepository = vehicleRepository;
        }

        // GET: Vehicles
        public IActionResult Index()
        {
            var vehicles = vehicleRepository.GetallVehicles();
            return View(vehicles);
        }

        // GET: Vehicles/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = vehicleRepository.Details(id);
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
        public IActionResult Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    vehicleRepository.Create(vehicle);
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
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = vehicleRepository.Details(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Vehicle vehicle)
        {
            if (id != vehicle.VehicleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vehicleRepository.Edit(id, vehicle);
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
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = vehicleRepository.Details(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            vehicleRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
