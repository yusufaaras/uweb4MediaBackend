    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using uweb4Media.Application.Features.CQRS.Commands.User;
    using uweb4Media.Application.Interfaces;
    using Uweb4Media.Domain.Entities;

    namespace uweb4Media.Application.Features.CQRS.Handlers.User
    {
        public class UpdateUserCommandHandler
        {
            private readonly IRepository<AppUser> _repository;
            public UpdateUserCommandHandler(IRepository<AppUser> repository)
            {
                _repository = repository;
            }

            public async Task Handle(UpdateUserCommand command)
            {
                var values = await _repository.GetByIdAsync(command.AppUserID);
                values.Username = command.Username;
                values.Password = command.Password;
                values.Name = command.Name;
                values.Surname = command.Surname;
                values.Email = command.Email;
                values.AppRoleID = command.AppRoleID.Value;
                values.SubscriptionStatus = command.SubscriptionStatus;
                values.PostToken = command.PostToken;
                values.AvatarUrl = command.AvatarUrl;
                values.Bio = command.Bio; 
                values.SubscriptionStartDate = command.SubscriptionStartDate;
                values.SubscriptionEndDate = command.SubscriptionEndDate;

                await _repository.UpdateAsync(values);
            }
        }
    }
