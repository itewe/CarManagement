using CarManagement.Data;
using CarManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarManagement.Repositories
{
    public class MaintenanceRepository
    {
        private readonly CarManagementContext _context;

        public MaintenanceRepository(CarManagementContext context)
        {
            _context = context;
        }

        //Get all maintenance records
        public IEnumerable<Maintenance> GetAllMaintenances()
        {
            return _context.Maintenances.Include(m => m.Vehicle).ToList();
        }

        public IEnumerable<Maintenance> GetAllMaintenancesByVehicleId(int id)
        {
            return _context.Maintenances
               .Where(m => m.VehicleId == id)
               .Include(m => m.Vehicle)
               .OrderByDescending(m => m.DateOfMaintenance)
               .ToList();
        }


        // Get details of a specific maintenance record by ID
        public Maintenance GetMaintenanceDetails(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var maintenance = _context.Maintenances
                .Include(m => m.Vehicle)
                .FirstOrDefault(m => m.MaintenanceId == id);

            return maintenance;
        }

        // Create a new maintenance record
        public void CreateMaintenance(Maintenance maintenance)
        {

            _context.Maintenances.Add(maintenance);
            _context.SaveChanges();

        }

        // Update an existing maintenance record
        public void EditMaintenance(int id, Maintenance maintenance)
        {

            var existingMaintenance = _context.Maintenances.Find(id);
            if (existingMaintenance != null)
            {
                _context.Entry(existingMaintenance).CurrentValues.SetValues(maintenance);
                _context.SaveChanges();
            }


        }

        // Delete a maintenance record
        public void DeleteMaintenance(int id)
        {

            var maintenance = _context.Maintenances.Find(id);
            if (maintenance != null)
            {
                _context.Maintenances.Remove(maintenance);
                _context.SaveChanges();
            }
        }
    }
}
