using MediatR;

namespace uweb4Media.Application.Features.Mediator.Commands.GoogleCommands;

public class CreateGoogleAppUserCommand : IRequest<bool>
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string GoogleId { get; set; } 
    public string? AvatarUrl { get; set; } 
}