using MediatR;

namespace uweb4Media.Application.Features.Mediator.Commands.Github;

public class CreateGithubAppUserCommand : IRequest<int>
{
    public string Email { get; set; }
    public string GithubId { get; set; }
    public string Username { get; set; }
    public string AvatarUrl { get; set; }
}