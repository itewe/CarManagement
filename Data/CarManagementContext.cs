using CarManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarManagement.Data
{
    public class CarManagementContext : DbContext
    {
        public CarManagementContext(DbContextOptions<CarManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<VehicleDriverHistory> VehicleDriverHistories { get; set; }
    }
}
