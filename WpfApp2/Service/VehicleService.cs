using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Model;

namespace WpfApp2.Service
{
    public class VehicleService
    {
        private readonly AppDbContext _context;
        public VehicleService() => _context = new AppDbContext();

        // Lấy tất cả Vehicle 
        public async Task<List<Vehicle>> GetAllVehiclesAsync() =>
            await _context.Set<Vehicle>()
                .AsNoTracking()
                .ToListAsync();

        // Lấy Vehicle theo Id
        public async Task<Vehicle?> GetVehicleByIdAsync(string id) =>
            await _context.Vehicle
                    .AsNoTracking()
                    .Include(v => v.Person)
                    .Include(v => v.Brand)
                    .Include(v => v.Type)
                    .FirstOrDefaultAsync(v => v.Id == id);

        // Lấy danh sách Vehicle theo OwnerId
        public async Task<List<Vehicle>> GetVehiclesByOwnerAsync(string ownerId) =>
            await _context.Set<Vehicle>()
                .AsNoTracking()
                .Where(v => v.OwnerId == ownerId)
                .ToListAsync();

        // Lấy danh sách Vehicle theo BrandId
        public async Task<List<Vehicle>> GetVehiclesByBrandAsync(int brandId) =>
            await _context.Set<Vehicle>()
                .AsNoTracking()
                .Where(v => v.BrandId == brandId)
                .ToListAsync();

        // Lấy danh sách Vehicle theo TypeId
        public async Task<List<Vehicle>> GetVehiclesByTypeAsync(int typeId) =>
            await _context.Set<Vehicle>()
                .AsNoTracking()
                .Where(v => v.TypeId == typeId)
                .ToListAsync();
        //Lấy số lượng xe
        public async Task<int> GetVehicleCountAsync() =>
            await _context.Set<Vehicle>()
                
                .AsNoTracking()
                .Join(
                    _context.Report,
                    vehicle => vehicle.Id,
                    report => report.VehicleId,
                    (vehicle, report) => vehicle
                )
                .Distinct() // Loại bỏ trùng lặp nếu cần
                .CountAsync();

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
