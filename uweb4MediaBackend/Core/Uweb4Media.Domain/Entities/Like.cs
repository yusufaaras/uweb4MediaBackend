namespace Uweb4Media.Domain.Entities
{
    public class Like
    {
        public int Id { get; set; } 
        public int UserId { get; set; } 
        public AppUser User { get; set; } 
        public int MediaContentId { get; set; } 
        public MediaContent MediaContent { get; set; }
        public DateTime LikeDate { get; set; } = DateTime.UtcNow;
    }
    
}

