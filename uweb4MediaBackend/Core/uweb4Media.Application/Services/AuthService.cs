using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace Uweb4Media.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> RegisterAsync(string username, string email, string password)
        {
            Console.WriteLine($"🔄 RegisterAsync called for: {email}");

            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                Console.WriteLine($"❌ User already exists: {email} (ID: {existingUser.Id})");
                return null;
            }

            Console.WriteLine($"✅ Email available, creating user: {email}");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hashedPassword
            };

            Console.WriteLine($"💾 Adding user to database: {email}");
            await _userRepository.AddUserAsync(user);
            Console.WriteLine($"🎉 User successfully created: {email} (ID: {user.Id})");

            return user;
        }

        // ✅ Eksik: LoginAsync metodu olmalı!
        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            bool verified = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return verified ? user : null;
        }
    }
}