using MediatR;
using uweb4Media.Application.Features.Mediator.Commands.Github;
using uweb4Media.Application.Interfaces.AppUserInterfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.Mediator.Handlers.GitHub;

public class CreateGithubAppUserCommandHandler : IRequestHandler<CreateGithubAppUserCommand, int>
{
    private readonly IAppUserRepository _userRepository;
    public CreateGithubAppUserCommandHandler(IAppUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<int> Handle(CreateGithubAppUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByGithubIdAsync(request.GithubId);
        if (existing != null) return existing.AppUserID;

        var user = new AppUser
        {
            Email = request.Email,
            GithubId = request.GithubId,
            Username = request.Username,
            AvatarUrl = request.AvatarUrl,
            SubscriptionStatus = "free"
        };
        await _userRepository.AddAsync(user);
        return user.AppUserID;
    }
}