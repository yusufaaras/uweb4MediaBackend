using MediatR;
using uweb4Media.Application.Enums;
using uweb4Media.Application.Features.Mediator.Commands.GoogleCommands;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.Mediator.Handlers.GoogleHandlers;

public class CreateGoogleAppUserCommandHandler : IRequestHandler<CreateGoogleAppUserCommand, bool>
{
    private readonly IRepository<AppUser> _repository;

    public CreateGoogleAppUserCommandHandler(IRepository<AppUser> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(CreateGoogleAppUserCommand request, CancellationToken cancellationToken)
    { 
        var existingUser = await _repository.GetByFilterAsync(
            x => x.Email == request.Email || (x.GoogleId != null && x.GoogleId == request.GoogleId));

        if (existingUser == null)
        {
            // Yeni kullanıcı kaydı
            await _repository.CreateAsync(new AppUser
            {
                Username = request.Email.Split('@')[0],
                Password = null,
                AppRoleID = (int)RolesType.Member, // ← GERİ ALDI! (2 olacak)
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                GoogleId = request.GoogleId, 
                AvatarUrl = request.AvatarUrl,
                SubscriptionStatus = "free"
            });
            return true;
        }
        else
        {
            // Mevcut kullanıcıyı güncelle (örneğin profil resmi değişmiş olabilir)
            existingUser.Name = request.Name;
            existingUser.Surname = request.Surname;
            existingUser.AvatarUrl = request.AvatarUrl;
            existingUser.GoogleId = request.GoogleId; // Eğer yoksa veya değişmişse güncelle

            await _repository.UpdateAsync(existingUser);
            return true;
        }
    }
}