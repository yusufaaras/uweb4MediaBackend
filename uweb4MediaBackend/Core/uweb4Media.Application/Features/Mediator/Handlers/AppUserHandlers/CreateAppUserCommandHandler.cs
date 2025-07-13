using BCrypt.Net;
using MediatR;
using uweb4Media.Application.Enums;
using uweb4Media.Application.Features.Mediator.Commands.AppUserCommands;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities; 

public class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand>
{
    private readonly IRepository<AppUser> _repository;
    public CreateAppUserCommandHandler(IRepository<AppUser> repository)
    {
        _repository = repository;
    }

    public async Task Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _repository.GetByFilterAsync(
            x => x.Username == request.Username || x.Email == request.Email);

        if (existingUser != null)
        {
            throw new Exception("Registration cannot be made with the same username or email address.");
        }
         
        if (string.IsNullOrEmpty(request.Email) || !request.Email.Contains("@"))
        {
            throw new Exception("Enter a valid email address. (There must be an @ in the email)");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        await _repository.CreateAsync(new AppUser
        {
            Username = request.Username,
            Password = passwordHash, 
            AppRoleID = (int)RolesType.Member,
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email
        });
    }
}