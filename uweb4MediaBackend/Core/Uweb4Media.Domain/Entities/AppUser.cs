using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uweb4Media.Domain.Entities.Admin.Campaign; 
using Uweb4Media.Domain.Entities.Admin.CompanyManagement;

namespace Uweb4Media.Domain.Entities
{
    public class AppUser
    {
        [Key]  
        public int AppUserID { get; set; }

        [Required] 
        [MaxLength(50)]  
        public string Username { get; set; }

        [Required] 
        public string Password { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; } 

        [MaxLength(50)]
        public string? Surname { get; set; }  

        public string? AvatarUrl { get; set; }  

        [Required]  
        [MaxLength(20)]
        public string SubscriptionStatus { get; set; } = "free";  

        public string? Bio { get; set; } 

        [Required]  
        [EmailAddress]  
        [MaxLength(100)]  
        public string Email { get; set; }
        
        [MaxLength(6)]
        public string? EmailVerificationCode { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        [MaxLength(256)] 
        public string? GoogleId { get; set; }
        public int AppRoleID { get; set; }  
        [ForeignKey("AppRoleID")] 
        public AppRole AppRole { get; set; }   
        public ICollection<MediaContent> MediaContents { get; set; } 
        public ICollection<Comment> Comments { get; set; } 
        public ICollection<Like> Likes { get; set; } 
        public ICollection<Subscription> SubscriptionsMade { get; set; }
        public ICollection<Subscription> SubscriptionsReceived { get; set; }
        public ICollection<Campaign> Campaigns { get; set; } 
        public ICollection<Company> Companies { get; set; } 
        public ICollection<Notification> Notifications { get; set; } 

    }
}