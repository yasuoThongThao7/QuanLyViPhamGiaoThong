using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Model;

namespace WpfApp2.Service
{
    public class ViolationService
    {
        private readonly AppDbContext _context;
        public ViolationService() => _context = new AppDbContext();
       

        // Tìm kiếm vi phạm theo tên hoặc mã vi phạm  
        public async Task<List<Violation>> SearchViolationsAsync(string searchText)
            => await _context.Violation
                .Where(v => v.Description!.Contains(searchText) || v.Id.ToString().Contains(searchText))
                .ToListAsync();

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
        

        public async Task<List<Violation>> GetAllViolationsAsync()
            => await _context.Violation.AsNoTracking().Include(v => v.Type).ToListAsync();

        // Lấy danh sách vi phạm theo 1 mảng ID  
        public async Task<List<Violation>> GetViolationByIdRangeAsync(int[] id)
        {
            if (id == null || id.Length == 0)
                return new List<Violation>();
            var idSet = new HashSet<int>(id);
            return await _context.Violation
                .AsNoTracking()
                .Where(v => idSet.Contains(v.Id))
                .ToListAsync();
        }
    }
}
