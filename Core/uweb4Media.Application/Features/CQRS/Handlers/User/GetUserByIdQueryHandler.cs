using uweb4Media.Application.Features.CQRS.Queries.User;
using uweb4Media.Application.Features.CQRS.Results.User;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;
using uweb4Media.Application.Services.User; // UserService için eklendi

namespace uweb4Media.Application.Features.CQRS.Handlers.User
{
    public class GetUserByIdQueryHandler
    {
        private readonly IRepository<AppUser> _repository;
        private readonly UserService _userService;

        public GetUserByIdQueryHandler(IRepository<AppUser> repository, UserService userService)
        {
            _repository = repository;
            _userService = userService;
        }

        public async Task<GetUserByIdQueryResult> Handle(GetUserByIdQuery query)
        {
            var values = await _repository.GetByIdAsync(query.Id);
 
            await _userService.CheckAndUpdateSubscriptionAsync(values);

            return new GetUserByIdQueryResult
            {
                AppUserID = values.AppUserID,
                Username = values.Username,
                Password = values.Password,
                Name = values.Name,
                Surname = values.Surname,
                Email = values.Email,
                SubscriptionStartDate = values.SubscriptionStartDate,
                SubscriptionEndDate = values.SubscriptionEndDate,
                AppRoleID = values.AppRoleID,
                AvatarUrl = values.AvatarUrl,
                Bio = values.Bio,
                SubscriptionStatus = values.SubscriptionStatus,
                PostToken = values.PostToken,
            };
        }
    }
}