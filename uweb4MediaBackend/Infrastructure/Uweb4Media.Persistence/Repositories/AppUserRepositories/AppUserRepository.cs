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
        private readonly WeddingHallContext _context;
        public AppUserRepository(WeddingHallContext context)
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
    }
}
