using MediatR;
using Uweb4Media.Application.Features.MediaContents.Dtos;
using Uweb4Media.Domain.Enums;

namespace Uweb4Media.Application.Features.MediaContents.Commands;

public class CreateMediaContentCommand : IRequest<MediaContentDto>
{
    public string Title { get; set; } = null!;
    public string ThumbnailUrl { get; set; } = null!;
    public Sector Sector { get; set; }
    public Channel Channel { get; set; }
    public ContentType ContentType { get; set; }
    public string? YoutubeVideoId { get; set; }
    public Guid UserId { get; set; }
}