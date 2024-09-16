using CarManagement.Data;
using CarManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    public class VehicleRepository
    {
        private readonly CarManagementContext _context;
        private readonly VehicleDriverHistoryRepository _historyRepository;

        public VehicleRepository(CarManagementContext context, VehicleDriverHistoryRepository historyRepository)
        {
            _context = context;
            _historyRepository = historyRepository;
        }

        // GET: Vehicles
        public async Task<IEnumerable<Vehicle>> GetallVehicles()
        {
            return await _context.Vehicles
                .Include(v => v.CurrentDriver)
                .ToListAsync();
        }

        // GET: Vehicles/Details/5
        public async Task<Vehicle> Details(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await _context.Vehicles
                .Include(v => v.CurrentDriver)
                .FirstOrDefaultAsync(v => v.VehicleId == id);
        }

        // Create a new Vehicle
        public async Task Create(Vehicle vehicle)
        {
            try
            {
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UNIQUE") == true)
                {
                    throw new Exception("A vehicle with the same Plate Number already exists.");
                }
                else
                {
                    throw;
                }
            }
        }

        // Edit an existing Vehicle
        public async Task Edit(int id, Vehicle vehicle)
        {
            try
            {
                var existingVehicle = await _context.Vehicles.FindAsync(id);
                if (existingVehicle != null)
                {
                    existingVehicle.NumberOfSeats = vehicle.NumberOfSeats;
                    existingVehicle.PlateNumber = vehicle.PlateNumber;
                    existingVehicle.Color = vehicle.Color;
                    existingVehicle.Type = vehicle.Type;

                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UNIQUE") == true)
                {
                    throw new Exception("A vehicle with the same Plate Number already exists.");
                }
                else
                {
                    throw;
                }
            }
        }

        // Delete a vehicle
        public async Task Delete(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {id} not found.");
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        // Update driver for a vehicle
        public async Task UpdateDriver(int vehicleId, int driverId)
        {
            // Find the vehicle
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");
            }

            // Find the last vehicle-driver history for this vehicle where EndDate is null
            var lastHistory = (await _historyRepository.GetVehicleDriverHistoriesByVehicleId(vehicleId))
                .FirstOrDefault(vdh => vdh.EndDate == null);

            // If a history entry is found with null EndDate, set its EndDate to the current date and time
            if (lastHistory != null)
            {
                lastHistory.EndDate = DateTime.Now;
                await _historyRepository.EditVehicleDriverHistory(lastHistory.VehicleDriverHistoryId, lastHistory);
            }

            // Handle case where driverId is set to 0, indicating no current driver
            if (driverId == 0)
            {
                vehicle.CurrentDriverId = null;

                await _context.SaveChangesAsync();
                return;
            }

            // Find the driver
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driverId} not found.");
            }

            // Set the new current driver
            vehicle.CurrentDriverId = driverId;

            // Create a new vehicle-driver history for the new driver
            var newHistory = new VehicleDriverHistory
            {
                VehicleId = vehicleId,
                DriverId = driverId,
                AssignmentDate = DateTime.Now,
                Description = $"Assigned driver {driver.LicenseNumber} to vehicle {vehicle.PlateNumber}"
            };

            // Add the new history entry
            await _historyRepository.CreateVehicleDriverHistory(newHistory);

            // Save the vehicle changes (assign new driver)
            await _context.SaveChangesAsync();
        }
    }
}
