using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(string username, string email, string password);
    Task<User?> LoginAsync(string username, string password);
}