using Microsoft.EntityFrameworkCore;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace Uweb4Media.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // public async Task<User?> GetByUsernameAsync(string username)
        // {
        //     return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        // }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}