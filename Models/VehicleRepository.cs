using CarManagement.Data;
using CarManagement.Repositories;
using Microsoft.EntityFrameworkCore;

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
        public IEnumerable<Vehicle> GetallVehicles()
        {
            return _context.Vehicles
                               .Include(v => v.CurrentDriver)
                               .ToList();
        }

        // GET: Vehicles/Details/5
        public Vehicle Details(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var vehicle = _context.Vehicles
                          .Include(v => v.CurrentDriver)
                          .FirstOrDefault(v => v.VehicleId == id);

            return vehicle;
        }

        // Create a new Vehicle
        public void Create(Vehicle vehicle)
        {
            try
            {
                _context.Vehicles.Add(vehicle);
                _context.SaveChanges();
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
        public void Edit(int id, Vehicle vehicle)
        {
            try
            {
                var existingVehicle = _context.Vehicles.Find(id);
                if (existingVehicle != null)
                {
                    existingVehicle.NumberOfSeats = vehicle.NumberOfSeats;
                    existingVehicle.PlateNumber = vehicle.PlateNumber;

                    existingVehicle.Color = vehicle.Color;
                    existingVehicle.Type = vehicle.Type;

                    _context.SaveChanges();
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

        public void Delete(int id)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {id} not found.");
            }

            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
        }
        public void UpdateDriver(int vehicleId, int driverId)
        {
            // Find the vehicle
            var vehicle = _context.Vehicles.Find(vehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");
            }

            // Find the last vehicle-driver history for this vehicle where EndDate is null
            var lastHistory = _historyRepository.GetVehicleDriverHistoriesByVehicleId(vehicleId)
                                                 .FirstOrDefault(vdh => vdh.EndDate == null);

            // If a history entry is found with null EndDate, set its EndDate to the current date and time
            if (lastHistory != null)
            {
                lastHistory.EndDate = DateTime.Now;
                _historyRepository.EditVehicleDriverHistory(lastHistory.VehicleDriverHistoryId, lastHistory);
            }

            // Handle case where driverId is set to 0, indicating no current driver
            if (driverId == 0)
            {
                vehicle.CurrentDriverId = null;

                _context.SaveChanges();
                return;
            }

            // Find the driver
            var driver = _context.Drivers.Find(driverId);
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
            _historyRepository.CreateVehicleDriverHistory(newHistory);

            // Save the vehicle changes (assign new driver)
            _context.SaveChanges();
        }

    }
}

