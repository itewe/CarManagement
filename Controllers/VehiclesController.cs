﻿using Microsoft.AspNetCore.Mvc;
using CarManagement.Data;
using CarManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        // GET: Vehicles/EditDriver/5
        public IActionResult EditDriver(int? id)
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

            var drivers = driversRepository.GetAllDrivers();

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
        public IActionResult EditDriver(EditDriverViewModel model)
        {
            try
            {
                vehicleRepository.UpdateDriver(model.VehicleId, model.SelectedDriverId);
                return Redirect($"/VehicleDriverHistories/HistorybyvehicleId/{model.VehicleId}");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the driver.");
            }
            var drivers = driversRepository.GetAllDrivers();
            model.Drivers = new SelectList(drivers, "DriverId", "LicenseNumber");
            return View(model);
        }



    }
}
