using CarManagement.Data;

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
        public IEnumerable<Driver> GetAllDrivers()
        {
            return _context.Drivers.ToList();
        }

        // GET: Drivers/Details/5
        public Driver Details(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return _context.Drivers.Find(id);
        }

        // Create a new Driver
        public void Create(Driver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            _context.Drivers.Add(driver);
            _context.SaveChanges();
        }

        // Edit an existing Driver
        public void Edit(int id, Driver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }
            if (id != driver.DriverId) return;

            var existingDriver = _context.Drivers.Find(driver.DriverId);
            if (existingDriver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driver.DriverId} not found.");
            }

            _context.Entry(existingDriver).CurrentValues.SetValues(driver);
            _context.SaveChanges();
        }

        // Delete a Driver
        public void Delete(int id)
        {
            var driver = _context.Drivers.Find(id);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {id} not found.");
            }

            _context.Drivers.Remove(driver);
            _context.SaveChanges();
        }
    }
}