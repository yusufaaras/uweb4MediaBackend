using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Services.User
{
    public class UserService
    {
        private readonly IRepository<AppUser> _repository;
        public UserService(IRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public async Task CheckAndUpdateSubscriptionAsync(AppUser user)
        {
            if (user.SubscriptionStatus == "premium" && user.SubscriptionEndDate.HasValue)
            {
                if (user.SubscriptionEndDate.Value < DateTime.UtcNow)
                {
                    user.SubscriptionStatus = "free";
                    user.SubscriptionStartDate = null;
                    user.SubscriptionEndDate = null;
                    await _repository.UpdateAsync(user);
                }
            }
        }
    }
}