using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uweb4Media.Domain.Entities;

public class MediaContent
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; } 

        [Required]
        public string Url { get; set; }

        [Required]
        [MaxLength(255)]  
        public string Title { get; set; }
        [Required]
        [MaxLength(50)]  
        public string Sector { get; set; } 
        [Required]
        [MaxLength(50)]  
        public string Channel { get; set; } 
        [Required]
        [MaxLength(20)] 
        public string ContentType { get; set; }
        public string Thumbnail { get; set; } 
        public int LikesCount { get; set; } = 0; 
        public int CommentsCount { get; set; } = 0; 
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;  
        public int ViewCount { get; set; } = 0;
        public bool IsPremium { get; set; } = false; 
        [MaxLength(255)]
        public string MetaTitle { get; set; } 
        public string MetaDescription { get; set; } 
        [MaxLength(20)]
        public string Duration { get; set; } 
        public string Excerpt { get; set; } 
        [MaxLength(50)]
        public string YoutubeVideoId { get; set; } 
        public string Tags { get; set; }
        public ICollection<Like> Likes { get; set; }  
        public ICollection<Comment> Comments { get; set; } 
    }