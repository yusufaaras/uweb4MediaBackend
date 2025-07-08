using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddUserAsync(User user);
}