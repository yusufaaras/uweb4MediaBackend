namespace Uweb4Media.Application.Features.MediaContents.Dtos;

public class MediaContentDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string ThumbnailUrl { get; set; } = null!;
    public string Sector { get; set; } = null!;
    public string Channel { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public string? YoutubeVideoId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}