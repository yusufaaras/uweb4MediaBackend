using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using uweb4Media.Application.Interfaces.AppRoleInterfaces;
using Uweb4Media.Domain.Entities;
using Uweb4Media.Persistence.Context;

namespace Uweb4Media.Persistence.Repositories.AppRoleRepositories
{
    public class AppRoleRepository : IAppRoleRepository
    {
        private readonly Uweb4MediaContext _context;
        public AppRoleRepository(Uweb4MediaContext context)
        {
            _context = context;
        }

        public async Task<AppRole> GetByFilterAsync(Expression<Func<AppRole, bool>> filter)
        {
            var values = await _context.AppRoles
                .Where(filter)
                .FirstOrDefaultAsync();
            return values;
        }
    }
}
