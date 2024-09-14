using Microsoft.EntityFrameworkCore;
using CarManagement.Models;

namespace CarManagement.Data
{
    public class CarManagementContext : DbContext
    {
        public CarManagementContext(DbContextOptions<CarManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<VehicleDriverHistory> VehicleDriverHistories { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure that PlateNumber is unique
            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.PlateNumber)
                .IsUnique();

            // Ensure that LicenseNumber is unique
            modelBuilder.Entity<Driver>()
                .HasIndex(d => d.LicenseNumber)
                .IsUnique();

            // Configure one-to-many relationship between Vehicle and Driver
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.CurrentDriver)
                .WithMany(d => d.CurrentVehicles)
                .HasForeignKey(v => v.CurrentDriverId)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}