namespace Uweb4Media.Domain.Entities;

public class MediaContent
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public AppUser User { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string Sector { get; set; }
    public string Channel { get; set; }
    public string ContentType { get; set; }
    public string Thumbnail { get; set; }
    public ICollection<Like> Likes { get; set; } 
    public ICollection<Comment> Comments { get; set; } 
}