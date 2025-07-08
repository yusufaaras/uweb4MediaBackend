using Uweb4Media.Domain.Enums;

namespace Uweb4Media.Domain.Entities
{
    public class MediaContent
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = null!;

        public Sector Sector { get; set; }
        public Channel Channel { get; set; }
        public ContentType ContentType { get; set; }

        public string ThumbnailUrl { get; set; } = null!;
        public string? YoutubeVideoId { get; set; }
        //user ile ilişkilendir
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        
        
        
    }
}