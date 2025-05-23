using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Model;

namespace WpfApp2.Service
{
    public class AccountService
    {
        private readonly AppDbContext _context;
        public AccountService() 
        {
            _context = new AppDbContext();
        }

        //Thêm account mới
        public async Task Add(Account account)
        {
            await _context.Account!.AddAsync(account);
            await _context.SaveChangesAsync();
        }
        //Tìm kiếm theo username
        public async Task<bool> SearchAsync(string username)
             => await _context.Account!
                .AsNoTracking()
                .AnyAsync(a => a.LoginId == username);
        

        //Thay đổi mk
        public async Task<bool> UpdatePasswordAsync(string id, string newPassword)
        {
            var affected = await _context.Account!
                .Where(a => a.LoginId == id)
                .ExecuteUpdateAsync(s => s.SetProperty(a => a.Password, _ => newPassword));
            return affected > 0;
        }

        //Check Password
        public async Task<Account?> CheckPasswordAsync(string id, string password)
        {
            var account = await _context.Account!
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(a => a.LoginId == id);

            if (account != null && account.Password == password)
            {
                return account;
            }

            return null;
        }


    }


}

