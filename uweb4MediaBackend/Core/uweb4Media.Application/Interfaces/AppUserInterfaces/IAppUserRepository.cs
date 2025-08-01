using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Interfaces.AppUserInterfaces
{
    public interface IAppUserRepository
    {
        Task<AppUser> GetByFilterAsync(Expression<Func<AppUser, bool>> filter);
        Task<AppUser?> GetByIdAsync(int id);
        Task<AppUser?> GetByEmailAsync(string email);
    }
}
