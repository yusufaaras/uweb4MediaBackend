namespace uweb4Media.Application.Features.CQRS.Commands.Media;

public class UpdateMediaContentCommand
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public string? Title { get; set; }
    public string? Sector { get; set; }
    public string? Channel { get; set; }
    public string? ContentType { get; set; }
    public string? Thumbnail { get; set; }
    public bool? IsPremium { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? Duration { get; set; }
    public string? Excerpt { get; set; }
    public string? YoutubeVideoId { get; set; }
    public string? Tags { get; set; }
}