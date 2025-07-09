using MediatR;
using Uweb4Media.Application.Features.MediaContents.Dtos;

namespace Uweb4Media.Application.Features.MediaContents.Queries;

public class GetAllMediaContentsQuery : IRequest<List<MediaContentDto>>
{
}