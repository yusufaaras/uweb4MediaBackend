namespace uweb4Media.Application.Features.CQRS.Commands.User
{
    public class UpdateUserCommand
    {
        public int AppUserID { get; set; } // ID is always required to identify the user
        public string? Username { get; set; } // Made nullable
        public string? Password { get; set; } // Made nullable
        public string? Name { get; set; }     // Made nullable
        public string? Surname { get; set; }  // Made nullable
        public string? Email { get; set; }    // Made nullable
        public int? AppRoleID { get; set; } // Made nullable
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        
        public int PostToken { get; set; } 
        public string? SubscriptionStatus { get; set; } // Made nullable
    }
}