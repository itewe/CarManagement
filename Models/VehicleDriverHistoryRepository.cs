using CarManagement.Data;
using CarManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarManagement.Repositories
{
    public class VehicleDriverHistoryRepository
    {
        private readonly CarManagementContext _context;

        public VehicleDriverHistoryRepository(CarManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VehicleDriverHistory>> GetAllVehicleDriverHistories()
        {
            return await _context.VehicleDriverHistories
                .Include(vdh => vdh.Vehicle)
                .Include(vdh => vdh.Driver)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleDriverHistory>> GetVehicleDriverHistoriesByVehicleId(int vehicleId)
        {
            return await _context.VehicleDriverHistories
                .Where(vdh => vdh.VehicleId == vehicleId)
                .Include(vdh => vdh.Vehicle)
                .Include(vdh => vdh.Driver)
                .OrderByDescending(vdh => vdh.AssignmentDate)
                .ToListAsync();
        }

        public async Task<VehicleDriverHistory> GetVehicleDriverHistoryDetails(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await _context.VehicleDriverHistories
                .Include(vdh => vdh.Vehicle)
                .Include(vdh => vdh.Driver)
                .FirstOrDefaultAsync(vdh => vdh.VehicleDriverHistoryId == id);
        }

        public async Task<IEnumerable<VehicleDriverHistory>> GetVehicleDriverHistoriesByDriverId(int driverId)
        {
            return await _context.VehicleDriverHistories
                .Where(vdh => vdh.DriverId == driverId)
                .Include(vdh => vdh.Vehicle)
                .Include(vdh => vdh.Driver)
                .OrderByDescending(vdh => vdh.AssignmentDate)
                .ToListAsync();
        }

        public async Task CreateVehicleDriverHistory(VehicleDriverHistory vehicleDriverHistory)
        {
            _context.VehicleDriverHistories.Add(vehicleDriverHistory);
            await _context.SaveChangesAsync();
        }

        public async Task EditVehicleDriverHistory(int id, VehicleDriverHistory vehicleDriverHistory)
        {
            var existingHistory = await _context.VehicleDriverHistories.FindAsync(id);
            if (existingHistory != null)
            {
                _context.Entry(existingHistory).CurrentValues.SetValues(vehicleDriverHistory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteVehicleDriverHistory(int id)
        {
            var vehicleDriverHistory = await _context.VehicleDriverHistories.FindAsync(id);
            if (vehicleDriverHistory != null)
            {
                _context.VehicleDriverHistories.Remove(vehicleDriverHistory);
                await _context.SaveChangesAsync();
            }
        }
    }
}
