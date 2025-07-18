using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uweb4Media.Domain.Entities
{
    public class Like
    {
        [Key]
        public int Id { get; set; } 
        public int UserId { get; set; } 
        [ForeignKey("UserId")]
        public AppUser User { get; set; } 
        public int MediaContentId { get; set; } 
        [ForeignKey("MediaContentId")]
        public MediaContent MediaContent { get; set; }
        public DateTime LikeDate { get; set; } = DateTime.UtcNow;
    }
    
}

