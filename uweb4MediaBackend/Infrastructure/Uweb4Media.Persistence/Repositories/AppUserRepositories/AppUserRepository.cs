using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using uweb4Media.Application.Interfaces.AppUserInterfaces;
using Uweb4Media.Domain.Entities;
using Uweb4Media.Persistence.Context;

namespace Uweb4Media.Persistence.Repositories.AppUserRepositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly Uweb4MediaContext _context;
        public AppUserRepository(Uweb4MediaContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetByFilterAsync(Expression<Func<AppUser, bool>> filter)
        {
            var values = await _context.AppUsers
                .Where(filter)
                .Include(r => r.AppRole)
                .FirstOrDefaultAsync();
            return values;
        }

        public async Task<AppUser?> GetByIdAsync(int id)
        {
            return await _context.AppUsers
                .Include(r => r.AppRole)
                .FirstOrDefaultAsync(x => x.AppUserID == id);
        }

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateAsync(AppUser user) // EKLENDİ
        {
            _context.AppUsers.Update(user);
            await _context.SaveChangesAsync();
        }
    }
    
}
