using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarManagement.Models;
using CarManagement.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace CarManagement.Controllers
{
    [Authorize]
    public class VehicleDriverHistoriesController : Controller
    {
        private readonly VehicleDriverHistoryRepository vehicleDriverHistoryRepository;
        private readonly VehicleRepository vehicleRepository;
        private readonly DriversRepository driversRepository;

        public VehicleDriverHistoriesController(VehicleDriverHistoryRepository vehicleDriverHistoryRepository, VehicleRepository vehicleRepository, DriversRepository driversRepository)
        {
            this.vehicleRepository = vehicleRepository;
            this.driversRepository = driversRepository;
            this.vehicleDriverHistoryRepository = vehicleDriverHistoryRepository;
        }

        // GET: VehicleDriverHistories
        //public IActionResult Index()
        //{
        //    var vehicleDriverHistories = vehicleDriverHistoryRepository.GetAllVehicleDriverHistories();
        //    return View(vehicleDriverHistories);
        //}

        public async Task<IActionResult> HistorybyvehicleId(int id)
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

            ViewData["VehicleId"] = id;
            ViewData["Vehiclepltnb"] = vehicle.PlateNumber;
            var vehicleDriverHistories = await vehicleDriverHistoryRepository.GetVehicleDriverHistoriesByVehicleId(id);
            return View("Index", vehicleDriverHistories);
        }

        // GET: VehicleDriverHistories/DriverHistory/5
        public async Task<IActionResult> DriverHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await driversRepository.Details(id.Value);
            if (driver == null)
            {
                return NotFound();
            }

            // Set the driver's license number and driver ID in the ViewData for the view
            ViewData["DriverLicenseNb"] = driver.LicenseNumber;
            ViewData["DriverId"] = driver.DriverId;

            var history = await vehicleDriverHistoryRepository.GetVehicleDriverHistoriesByDriverId(id.Value);

            return View(history);  // Return the view with the driver's history
        }

        // GET: VehicleDriverHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleDriverHistory = await vehicleDriverHistoryRepository.GetVehicleDriverHistoryDetails(id);
            if (vehicleDriverHistory == null)
            {
                return NotFound();
            }

            return View(vehicleDriverHistory);
        }

        //// GET: VehicleDriverHistories/Create
        //public IActionResult Create(int id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var vehicle = vehicleRepository.Details(id);
        //    if (vehicle == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["DriverId"] = new SelectList(driversRepository.GetAllDrivers(), "DriverId", "Name");
        //    VehicleDriverHistory history = new VehicleDriverHistory
        //    {
        //        VehicleId = id
        //    };
        //    return View(history);
        //}

        //// POST: VehicleDriverHistories/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(VehicleDriverHistory vehicleDriverHistory)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Console.WriteLine(vehicleDriverHistory.VehicleId);
        //        var vehicle = vehicleRepository.Details(vehicleDriverHistory.VehicleId);
        //        if (vehicle == null)
        //        {
        //            return NotFound();
        //        }
        //        vehicleDriverHistoryRepository.CreateVehicleDriverHistory(vehicleDriverHistory);
        //        return RedirectToAction(nameof(HistorybyvehicleId), new { id = vehicle.VehicleId });
        //    }
        //    ViewData["DriverId"] = new SelectList(driversRepository.GetAllDrivers(), "DriverId", "Name", vehicleDriverHistory.DriverId);
        //    return View(vehicleDriverHistory);
        //}

        // GET: VehicleDriverHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleDriverHistory = await vehicleDriverHistoryRepository.GetVehicleDriverHistoryDetails(id);
            if (vehicleDriverHistory == null)
            {
                return NotFound();
            }

            ViewData["DriverId"] = new SelectList(await driversRepository.GetAllDrivers(), "DriverId", "Name", vehicleDriverHistory.DriverId);
            return View(vehicleDriverHistory);
        }

        // POST: VehicleDriverHistories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleDriverHistory vehicleDriverHistory)
        {
            if (id != vehicleDriverHistory.VehicleDriverHistoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await vehicleDriverHistoryRepository.EditVehicleDriverHistory(id, vehicleDriverHistory);
                }
                catch (Exception)
                {
                    var vehiclehistory = await vehicleDriverHistoryRepository.GetVehicleDriverHistoryDetails(id);
                    if (vehiclehistory == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(HistorybyvehicleId), new { id = vehicleDriverHistory.VehicleId });
            }

            ViewData["DriverId"] = new SelectList(await driversRepository.GetAllDrivers(), "DriverId", "Name", vehicleDriverHistory.DriverId);
            return View(vehicleDriverHistory);
        }

        // GET: VehicleDriverHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleDriverHistory = await vehicleDriverHistoryRepository.GetVehicleDriverHistoryDetails(id);
            if (vehicleDriverHistory == null)
            {
                return NotFound();
            }

            return View(vehicleDriverHistory);
        }

        // POST: VehicleDriverHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicleDriverHistory = await vehicleDriverHistoryRepository.GetVehicleDriverHistoryDetails(id);

            if (vehicleDriverHistory.EndDate == null)
            {
                ModelState.AddModelError(string.Empty, "You cannot delete this record because the driver is currently assigned. Please change the current driver before deleting.");
                return View(vehicleDriverHistory);
            }

            await vehicleDriverHistoryRepository.DeleteVehicleDriverHistory(id);
            return RedirectToAction(nameof(HistorybyvehicleId), new { id = vehicleDriverHistory.VehicleId });
        }
    }
}
