using CarManagement.Data;
using CarManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarManagement.Repositories
{
    public class VehicleDriverHistoryRepository
    {
        private readonly CarManagementContext _context;

        public VehicleDriverHistoryRepository(CarManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<VehicleDriverHistory> GetAllVehicleDriverHistories()
        {
            return _context.VehicleDriverHistories
                           .Include(vdh => vdh.Vehicle)
                           .Include(vdh => vdh.Driver)
                           .ToList();
        }

        public IEnumerable<VehicleDriverHistory> GetVehicleDriverHistoriesByVehicleId(int vehicleId)
        {
            return _context.VehicleDriverHistories
                           .Where(vdh => vdh.VehicleId == vehicleId)
                           .Include(vdh => vdh.Vehicle)
                           .Include(vdh => vdh.Driver)
                           .OrderByDescending(vdh => vdh.AssignmentDate)
                           .ToList();
        }

        public VehicleDriverHistory GetVehicleDriverHistoryDetails(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var vehicleDriverHistory = _context.VehicleDriverHistories
                                               .Include(vdh => vdh.Vehicle)
                                               .Include(vdh => vdh.Driver)
                                               .FirstOrDefault(vdh => vdh.VehicleDriverHistoryId == id);

            return vehicleDriverHistory;
        }
        public IEnumerable<VehicleDriverHistory> GetVehicleDriverHistoriesByDriverId(int driverId)
        {
            return _context.VehicleDriverHistories
                           .Where(vdh => vdh.DriverId == driverId)
                           .Include(vdh => vdh.Vehicle)
                           .Include(vdh => vdh.Driver)
                           .OrderByDescending(vdh => vdh.AssignmentDate)
                           .ToList();
        }
        public void CreateVehicleDriverHistory(VehicleDriverHistory vehicleDriverHistory)
        {
            _context.VehicleDriverHistories.Add(vehicleDriverHistory);
            _context.SaveChanges();
        }

        public void EditVehicleDriverHistory(int id, VehicleDriverHistory vehicleDriverHistory)
        {
            var existingHistory = _context.VehicleDriverHistories.Find(id);
            if (existingHistory != null)
            {
                _context.Entry(existingHistory).CurrentValues.SetValues(vehicleDriverHistory);
                _context.SaveChanges();
            }
        }

        public void DeleteVehicleDriverHistory(int id)
        {
            var vehicleDriverHistory = _context.VehicleDriverHistories.Find(id);
            if (vehicleDriverHistory != null)
            {
                _context.VehicleDriverHistories.Remove(vehicleDriverHistory);
                _context.SaveChanges();
            }
        }
    }
}
