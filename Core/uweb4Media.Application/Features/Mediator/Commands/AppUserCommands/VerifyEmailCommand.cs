using MediatR;

namespace uweb4Media.Application.Features.Mediator.Commands.AppUserCommands;

public class VerifyEmailCommand : IRequest<bool>
{
    public string Email { get; set; }
    public string VerificationCode { get; set; }
}