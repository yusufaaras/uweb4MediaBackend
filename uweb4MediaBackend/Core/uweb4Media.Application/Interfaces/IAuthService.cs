using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Interfaces;

public interface IAuthService
{
    Task<User?> RegisterAsync(string username, string email, string password);
    Task<User?> LoginAsync(string email, string password);
}
