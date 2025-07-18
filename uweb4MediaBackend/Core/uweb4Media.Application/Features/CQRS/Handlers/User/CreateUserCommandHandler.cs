using uweb4Media.Application.Features.CQRS.Commands.User;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.User;

public class CreateUserCommandHandler
{
    private IRepository<AppUser> _repository;

    public CreateUserCommandHandler(IRepository<AppUser> repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreateUserCommand command)
    {
        await _repository.CreateAsync(new AppUser
        { 
            Username = command.Username,
            Password= command.Password,
            Email = command.Email,
            AvatarUrl = command.AvatarUrl,
            Bio = command.Bio,
            Name = command.Name,
            Surname = command.Surname,  
        });
    }
}