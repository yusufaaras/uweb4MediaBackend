namespace Uweb4Media.Domain.Entities;

public class Comment
{
    public int Id { get; set; } 
    public string Text { get; set; } 
    public DateTime CommentDate { get; set; } = DateTime.UtcNow; 
    public int UserId { get; set; } 
    public AppUser User { get; set; } 
    public int MediaContentId { get; set; } 
    public MediaContent MediaContent { get; set; } 
}