using MediatR;
using uweb4Media.Application.Features.Mediator.Results.AppUserResults;

namespace uweb4Media.Application.Features.Mediator.Queries.GoogleQueries;

public class GetAppUserByGoogleIdQuery : IRequest<GetCheckAppUserQueryResult>
{
    public string GoogleId { get; set; }
    public string Email { get; set; } // E-posta da gerekebilir
}