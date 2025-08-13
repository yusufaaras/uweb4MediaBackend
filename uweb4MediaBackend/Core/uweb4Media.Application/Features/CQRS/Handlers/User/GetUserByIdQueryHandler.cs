
using uweb4Media.Application.Features.CQRS.Queries.User;
using uweb4Media.Application.Features.CQRS.Results.User;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.User
{
    public class GetUserByIdQueryHandler
    {
        private readonly IRepository<AppUser> _repository;
        public GetUserByIdQueryHandler(IRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public async Task<GetUserByIdQueryResult> Handle(GetUserByIdQuery query)
        {
            var values = await _repository.GetByIdAsync(query.Id);
            return new GetUserByIdQueryResult
            {
                AppUserID = values.AppUserID,
                Username = values.Username,
                Password = values.Password,
                Name = values.Name,
                Surname = values.Surname,
                Email = values.Email,
                AppRoleID = values.AppRoleID,
                AvatarUrl = values.AvatarUrl,
                Bio = values.Bio,
                SubscriptionStatus = values.SubscriptionStatus,
                PostToken = values.PostToken,
            };
        }
    }
}
