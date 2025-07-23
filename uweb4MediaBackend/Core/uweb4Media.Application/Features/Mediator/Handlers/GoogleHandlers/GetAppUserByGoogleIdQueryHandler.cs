// uweb4Media.Application/Features/Mediator/Handlers/GoogleHandlers/GetAppUserByGoogleIdQueryHandler.cs
using MediatR;
using uweb4Media.Application.Features.Mediator.Queries.GoogleQueries; // Bu using'in doğru olduğundan emin olun
using uweb4Media.Application.Features.Mediator.Results.AppUserResults;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace uweb4Media.Application.Features.Mediator.Handlers.GoogleHandlers;

public class GetAppUserByGoogleIdQueryHandler : IRequestHandler<GetAppUserByGoogleIdQuery, GetCheckAppUserQueryResult>
{
    private readonly IRepository<AppUser> _repository;

    public GetAppUserByGoogleIdQueryHandler(IRepository<AppUser> repository)
    {
        _repository = repository;
    }

    public async Task<GetCheckAppUserQueryResult> Handle(GetAppUserByGoogleIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByFilterAsync(
            x => x.GoogleId == request.GoogleId || x.Email == request.Email,
            x => x.AppRole 
        );

        if (user == null)
        {
            return new GetCheckAppUserQueryResult { IsExits = false };
        }

        // Google ile giriş yapan kullanıcıların parolası null olacağı için IsExits true dönecek ama parola kontrolü yapılmayacak
        return new GetCheckAppUserQueryResult
        {
            AppUserID = user.AppUserID,
            Username = user.Username,
            Email = user.Email, // Email alanı eklendi
            IsExits = true,
            Role = user.AppRole.Name, // AppRole ilişkisi yüklendikten sonra RoleName'e erişilebilir
            GoogleId = user.GoogleId, // Ek bilgiler
            AvatarUrl = user.AvatarUrl,
            Name = user.Name,
            Surname = user.Surname
        };
    }
}