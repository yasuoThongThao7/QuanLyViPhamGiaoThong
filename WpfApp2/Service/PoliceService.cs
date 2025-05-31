using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Model;

namespace WpfApp2.Service
{
    public class PoliceService
    {
        public AppDbContext _context;
        public PoliceService() => _context = new AppDbContext();
        // Tra police theo id
        public async Task<Police?> GetPoliceAsync (string id)
            => _ = await _context.Police!
                            .AsNoTracking()
                            .Include(p => p.Person)
                            .FirstOrDefaultAsync(p => p.MilitaryID == id);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
