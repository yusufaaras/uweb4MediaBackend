using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uweb4Media.Application.Features.Mediator.Results.AppUserResults
{
    public class GetCheckAppUserQueryResult
    {
        public int AppUserID { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }  
        public string? Role { get; set; }  
        public bool IsExits { get; set; } 
        public string? GoogleId { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
