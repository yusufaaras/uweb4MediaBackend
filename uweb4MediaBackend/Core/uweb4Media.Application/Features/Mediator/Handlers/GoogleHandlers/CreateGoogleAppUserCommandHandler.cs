using System.Text.RegularExpressions;
using MediatR;
using uweb4Media.Application.Enums;
using uweb4Media.Application.Features.Mediator.Commands.GoogleCommands;
using uweb4Media.Application.Helper;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.Mediator.Handlers.GoogleHandlers
{
    public class CreateGoogleAppUserCommandHandler : IRequestHandler<CreateGoogleAppUserCommand, bool>
    {
        private readonly IRepository<AppUser> _repository;

        public CreateGoogleAppUserCommandHandler(IRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CreateGoogleAppUserCommand request, CancellationToken cancellationToken)
        {
            // E-posta domain kontrolü EKLENDİ!
            if (!EmailDomainHelper.IsAllowedDomain(request.Email))
                throw new Exception("Bu e-posta servis sağlayıcısı ile kayıt yapılamaz."); 
            var googleId = request.GoogleId?.Trim();
            var name = request.Name?.Trim();
            var surname = request.Surname?.Trim();
            var avatarUrl = request.AvatarUrl?.Trim();
            var cleanName = Regex.Replace(name ?? "", "<.*?>", "");
            var cleanSurname = Regex.Replace(surname ?? "", "<.*?>", "");

            var existingUser = await _repository.GetByFilterAsync(
                x => x.Email == request.Email || (x.GoogleId != null && x.GoogleId == googleId));

            if (existingUser == null)
            {
                await _repository.CreateAsync(new AppUser
                {
                    Username = request.Email.Split('@')[0],
                    Password = null,
                    AppRoleID = (int)RolesType.Member,
                    Name = cleanName,
                    Surname = cleanSurname,
                    Email = request.Email,
                    GoogleId = googleId,
                    AvatarUrl = avatarUrl,
                    SubscriptionStatus = "free"
                });
                return true;
            }
            else
            {
                existingUser.Name = cleanName;
                existingUser.Surname = cleanSurname;
                existingUser.AvatarUrl = avatarUrl;
                existingUser.GoogleId = googleId;
                await _repository.UpdateAsync(existingUser);
                return true;
            }
        }
    }
}