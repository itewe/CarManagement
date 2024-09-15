using CarManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CarManagement.Models
{
    public class DriversRepository
    {
        private readonly CarManagementContext _context;

        public DriversRepository(CarManagementContext context)
        {
            _context = context;
        }

        // GET: Drivers
        public async Task<IEnumerable<Driver>> GetAllDrivers()
        {
            return await _context.Drivers.ToListAsync();
        }

        // GET: Drivers/Details/5
        public async Task<Driver> Details(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
            {
                driver.CurrentVehicles = await _context.Vehicles
                    .Where(v => v.CurrentDriverId == id)
                    .ToListAsync();
            }
            return driver;
        }

        // Create a new Driver
        public async Task Create(Driver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            try
            {
                await _context.Drivers.AddAsync(driver);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle duplicate License Number during edit
                if (ex.InnerException?.Message.Contains("UNIQUE") == true)
                {
                    throw new Exception("A Driver with the same Licence Number already exists.");
                }
                else
                {
                    throw;
                }
            }
        }

        // Edit an existing Driver
        public async Task Edit(int id, Driver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }
            if (id != driver.DriverId) return;

            try
            {
                var existingDriver = await _context.Drivers.FindAsync(driver.DriverId);
                if (existingDriver == null)
                {
                    throw new KeyNotFoundException($"Driver with ID {driver.DriverId} not found.");
                }

                _context.Entry(existingDriver).CurrentValues.SetValues(driver);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle duplicate License Number during edit
                if (ex.InnerException?.Message.Contains("UNIQUE") == true)
                {
                    throw new Exception("A Driver with the same Licence Number already exists.");
                }
                else
                {
                    throw;
                }
            }
        }

        // Delete a Driver
        public async Task Delete(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {id} not found.");
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAllCurrentVehicles()
        {
            return await _context.Vehicles
                .Where(v => v.CurrentDriverId != null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetCurrentVehiclesByDriverId(int driverId)
        {
            return await _context.Vehicles
                .Where(v => v.CurrentDriverId == driverId)
                .ToListAsync();
        }

        public async Task<Driver> GetDriverByLicenseNumber(string licenseNumber)
        {
            if (string.IsNullOrEmpty(licenseNumber))
            {
                throw new ArgumentException("License number must be provided.", nameof(licenseNumber));
            }

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.LicenseNumber == licenseNumber);

            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with license number {licenseNumber} not found.");
            }
            return driver;
        }
    }
}
