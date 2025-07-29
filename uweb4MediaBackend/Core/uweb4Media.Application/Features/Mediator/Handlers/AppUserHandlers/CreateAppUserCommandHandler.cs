using BCrypt.Net;
using MediatR;
using uweb4Media.Application.Enums;
using uweb4Media.Application.Features.Mediator.Commands.AppUserCommands;
using uweb4Media.Application.Interfaces;
using uweb4Media.Application.Interfaces.Email;
using Uweb4Media.Domain.Entities; 

// CreateAppUserCommandHandler.cs
public class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand>
{
    private readonly IRepository<AppUser> _repository;
    private readonly IEmailService _emailService;  

    public CreateAppUserCommandHandler(IRepository<AppUser> repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
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
        var verificationCode = new Random().Next(100000, 999999).ToString();

        var user = new AppUser
        {
            Username = request.Username,
            Password = passwordHash,
            AppRoleID = (int)RolesType.Member,
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            SubscriptionStatus = "free",
            EmailVerificationCode = verificationCode,
            IsEmailVerified = false
        };

        await _repository.CreateAsync(user);

        // E-posta gönder
        await _emailService.SendEmailAsync(request.Email, "Email Verification", $"Your verification code is: {verificationCode}");
    }
}