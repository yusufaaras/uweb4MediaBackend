using MediatR;
using Uweb4Media.Domain.Entities;
using Uweb4Media.Domain.Repositories;
using Uweb4Media.Application.Features.MediaContents.Dtos;

namespace Uweb4Media.Application.Features.MediaContents.Commands;

public class CreateMediaContentCommandHandler : IRequestHandler<CreateMediaContentCommand, MediaContentDto>
{
    private readonly IMediaContentRepository _mediaRepo;

    public CreateMediaContentCommandHandler(IMediaContentRepository mediaRepo)
    {
        _mediaRepo = mediaRepo;
    }

    public async Task<MediaContentDto> Handle(CreateMediaContentCommand request, CancellationToken cancellationToken)
    {
        var entity = new MediaContent
        {
            Title = request.Title,
            ThumbnailUrl = request.ThumbnailUrl,
            Sector = request.Sector,
            Channel = request.Channel,
            ContentType = request.ContentType,
            YoutubeVideoId = request.YoutubeVideoId,
            UserId = request.UserId
        };

        await _mediaRepo.AddAsync(entity);

        return new MediaContentDto
        {
            Id = entity.Id,
            Title = entity.Title,
            ThumbnailUrl = entity.ThumbnailUrl,
            Sector = entity.Sector.ToString(),
            Channel = entity.Channel.ToString(),
            ContentType = entity.ContentType.ToString(),
            YoutubeVideoId = entity.YoutubeVideoId,
            UserId = entity.UserId,
            CreatedAt = entity.CreatedAt
        };
    }
}