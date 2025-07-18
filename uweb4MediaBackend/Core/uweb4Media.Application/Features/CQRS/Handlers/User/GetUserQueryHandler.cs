using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uweb4Media.Application.Features.CQRS.Results.User;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.User
{
    public class GetUserQueryHandler
    {
        private readonly IRepository<AppUser> _repository;
        public GetUserQueryHandler(IRepository<AppUser> repository)
        {
            _repository = repository;
        }
        public async Task<List<GetUserQueryResult>> Handle()
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetUserQueryResult
            {
                AppUserID = x.AppUserID,
                Name = x.Name,
                Username = x.Username,
                Password = x.Password,
                Surname = x.Surname,
                Email = x.Email,
                AppRoleID = x.AppRoleID,
                AvatarUrl = x.AvatarUrl,
                Bio = x.Bio,
                SubscriptionStatus = x.SubscriptionStatus,
            }).ToList();
        }
    }

}
