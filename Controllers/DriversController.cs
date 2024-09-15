using Microsoft.AspNetCore.Mvc;
using CarManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarManagement.Controllers
{
    public class DriversController : Controller
    {
        private readonly DriversRepository _repository;

        public DriversController(DriversRepository repository)
        {
            _repository = repository;
        }

        // GET: Drivers
        public async Task<IActionResult> Index()
        {
            IEnumerable<Driver> drivers = await _repository.GetAllDrivers();
            return View(drivers);
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _repository.Details(id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // GET: Drivers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drivers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Driver driver)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.Create(driver);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Show the error message to the user
                    ModelState.AddModelError(string.Empty, "A Driver with the same Licence Number already exists.");
                }
            }
            return View(driver);
        }

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _repository.Details(id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Drivers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Driver driver)
        {
            if (id != driver.DriverId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.Edit(id, driver);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Show the error message to the user
                    ModelState.AddModelError(string.Empty, "A Driver with the same Licence Number already exists.");
                }
            }
            return View(driver);
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _repository.Details(id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _repository.Delete(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
