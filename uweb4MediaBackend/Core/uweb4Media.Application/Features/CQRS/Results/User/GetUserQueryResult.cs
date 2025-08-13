using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uweb4Media.Application.Features.CQRS.Results.User
{
    public class GetUserQueryResult
    {
        public int AppUserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        
        public int PostToken { get; set; } 
        public string Email { get; set; }
        public int AppRoleID { get; set; }
        public string? AvatarUrl { get; set; } 
        public string? Bio { get; set; }
        public string SubscriptionStatus { get; set; }
    }
}

