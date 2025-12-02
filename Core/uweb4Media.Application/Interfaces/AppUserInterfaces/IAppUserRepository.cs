using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Interfaces.AppUserInterfaces
{
    public interface IAppUserRepository
    {
        Task<AppUser> GetByFilterAsync(Expression<Func<AppUser, bool>> filter);
        Task<AppUser?> GetByIdAsync(int id);
        Task<AppUser?> GetByEmailAsync(string email);
        Task<AppUser?> GetByGithubIdAsync(string githubId);  
        Task<AppUser?> GetByGoogleIdAsync(string googleId); 
        Task<AppUser?> GetByMicrosoftIdAsync(string microsoftId);
        Task AddAsync(AppUser user);  
        Task UpdateAsync(AppUser user); 
    }
}