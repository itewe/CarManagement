using CarManagement.Data;

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
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
        }

        // Edit an existing Vehicle
        public void Edit(int id, Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle));
            }
            if (id != vehicle.VehicleId) return;

            // Check if the Vehicle exists
            var existingVehicle = _context.Vehicles.Find(vehicle.VehicleId);
            if (existingVehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicle.VehicleId} not found.");
            }

            _context.Entry(existingVehicle).CurrentValues.SetValues(vehicle);
            _context.SaveChanges();
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