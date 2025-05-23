using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Model;

namespace WpfApp2.Service
{
    public class PersonService
    {
        private readonly AppDbContext _context;

        public PersonService() =>_context = new AppDbContext();
        
        //Danh sách 
        public async Task<List<Person>> GetAllPersonsAsync()
            =>  await _context.Person!
                    .AsNoTracking()
                    .ToListAsync();
        // Tra 1 người vi phạm
        public async Task<Person?> GetPersonByIdAsync(string cccd)
             => await _context.Person!
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.CCCD == cccd);
        

        //Tra 1 người
        public async Task<bool> PersonExistsAsync(string cccd)
              => await _context.Person!
                .AsNoTracking()
                .AnyAsync(p => p.CCCD == cccd);
        // Lấy số lượng người vi phạm
        public async Task<int> GetPersonCountAsync()
        {
            return await _context.Person!
                .AsNoTracking()
                .Join(
                    _context.Report, 
                    person => person.CCCD, 
                    report => report.PersonId,
                    (person, report) => person 
                )
                .Distinct() // Loại bỏ trùng lặp nếu cần
                .CountAsync(); // Đếm số lượng
        }
    }
}
