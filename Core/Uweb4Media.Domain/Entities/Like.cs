using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uweb4Media.Domain.Entities.Admin.Video;

namespace Uweb4Media.Domain.Entities
{
    public class Like
    {
        [Key]
        public int Id { get; set; } 
        public int UserId { get; set; } 
        [ForeignKey("UserId")]
        public AppUser User { get; set; } 
        public int VideoId { get; set; } 
        [ForeignKey("VideoId")]
        public Video Video { get; set; }
        public DateTime LikeDate { get; set; } = DateTime.UtcNow;
    }
    
}

