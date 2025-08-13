// uweb4Media.Application/Features/Mediator/Handlers/AppUserHandlers/GetCheckAppUserQueryHandler.cs
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using uweb4Media.Application.Features.Mediator.Queries;
using uweb4Media.Application.Features.Mediator.Results.AppUserResults;
using uweb4Media.Application.Interfaces; // IRepository<T> için
using Uweb4Media.Domain.Entities; // AppUser için
using BCrypt.Net; // Parola doğrulaması için

namespace uweb4Media.Application.Features.Mediator.Handlers.AppUserHandlers
{
    public class GetCheckAppUserQueryHandler : IRequestHandler<GetCheckAppUserQuery, GetCheckAppUserQueryResult>
    {
        private readonly IRepository<AppUser> _userRepository;  

        public GetCheckAppUserQueryHandler(IRepository<AppUser> userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task<GetCheckAppUserQueryResult> Handle(GetCheckAppUserQuery request, CancellationToken cancellationToken)
        {
            var values = new GetCheckAppUserQueryResult();
 
            var user = await _userRepository.GetByFilterAsync(
                x => x.Username == request.UserName || x.Email == request.UserName,
                x => x.AppRole  
            );
 
            if (user == null || user.Password == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                values.IsExits = false;
            }
            else // Kullanıcı bulundu, şifresi var ve doğru
            {
                values.IsExits = true;
                values.AppUserID = user.AppUserID;
                values.Username = user.Username;
                values.Email = user.Email; // DTO'ya e-postayı atayın
                values.Role = user.AppRole?.Name; // İlişki yüklendiği için AppRole.Name'e güvenle erişebiliriz
                values.Name = user.Name; 
                values.Surname = user.Surname;
                values.AvatarUrl = user.AvatarUrl;
                values.GoogleId = user.GoogleId; // Normal kullanıcılar için null olacaktır, sorun değil
            }
            return values;
        }
    }
}