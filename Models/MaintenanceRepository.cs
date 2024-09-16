using CarManagement.Data;
using CarManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarManagement.Repositories
{
    public class MaintenanceRepository
    {
        private readonly CarManagementContext _context;

        public MaintenanceRepository(CarManagementContext context)
        {
            _context = context;
        }

        // Get all maintenance records
        public async Task<IEnumerable<Maintenance>> GetAllMaintenances()
        {
            return await _context.Maintenances.Include(m => m.Vehicle).OrderByDescending(m => m.DateOfMaintenance).ToListAsync();
        }

        // Get all maintenance records by VehicleId
        public async Task<IEnumerable<Maintenance>> GetAllMaintenancesByVehicleId(int id)
        {
            return await _context.Maintenances
               .Where(m => m.VehicleId == id)
               .Include(m => m.Vehicle)
               .OrderByDescending(m => m.DateOfMaintenance)
               .ToListAsync();
        }

        // Get details of a specific maintenance record by ID
        public async Task<Maintenance> GetMaintenanceDetails(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await _context.Maintenances
                .Include(m => m.Vehicle)
                .FirstOrDefaultAsync(m => m.MaintenanceId == id);
        }

        // Create a new maintenance record
        public async Task CreateMaintenance(Maintenance maintenance)
        {
            if (maintenance == null)
            {
                throw new ArgumentNullException(nameof(maintenance));
            }

            _context.Maintenances.Add(maintenance);
            await _context.SaveChangesAsync();
        }

        // Update an existing maintenance record
        public async Task EditMaintenance(int id, Maintenance maintenance)
        {
            if (maintenance == null)
            {
                throw new ArgumentNullException(nameof(maintenance));
            }

            var existingMaintenance = await _context.Maintenances.FindAsync(id);
            if (existingMaintenance != null)
            {
                _context.Entry(existingMaintenance).CurrentValues.SetValues(maintenance);
                await _context.SaveChangesAsync();
            }
        }

        // Delete a maintenance record
        public async Task DeleteMaintenance(int id)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance != null)
            {
                _context.Maintenances.Remove(maintenance);
                await _context.SaveChangesAsync();
            }
        }
    }
}
