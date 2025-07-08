using uweb4Media.Application.Enums;

namespace Uweb4Media.Domain.Entities

{
    public class User
    {
        
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public RolesType Role { get; set; } 
        
    }
}