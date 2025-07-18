namespace uweb4Media.Application.Features.CQRS.Handlers.Media;

public class GetMediaContentQueryResult
{
    public int Id { get; set; }
    public int UserId { get; set; } 
    public string Url { get; set; }
    public string Title { get; set; }
    public string Sector { get; set; }
    public string Channel { get; set; }
    public string ContentType { get; set; }
    public string Thumbnail { get; set; }
    
    public int Likes { get; set; }
    public int CommentsCount { get; set; }
    public DateTime Timestamp { get; set; }
    public int ViewCount { get; set; }
    public bool IsPremium { get; set; }
}