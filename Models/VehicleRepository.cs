using CarManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CarManagement.Models
{
    public class VehicleRepository
    {
        private readonly CarManagementContext _context;

        public VehicleRepository(CarManagementContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public IEnumerable<Vehicle> GetallVehicles()
        {
            return _context.Vehicles.ToList();
        }

        // GET: Vehicles/Details/5
        public Vehicle Details(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null)
            {
                return null;
            }

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
                // Log the error (ex) and handle it appropriately
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
                    _context.Entry(existingVehicle).CurrentValues.SetValues(vehicle);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                // Handle duplicate PlateNumber during edit
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

        // Delete a Vehicle
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
    }
}