namespace uweb4Media.Application.Features.Mediator.Commands.AppUserCommands;

using MediatR;

public class RegisterCompleteCommand : IRequest
{
    public string Email { get; set; }
    public string VerificationCode { get; set; }
}