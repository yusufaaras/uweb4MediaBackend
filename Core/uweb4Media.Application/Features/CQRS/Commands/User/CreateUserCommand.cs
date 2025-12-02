namespace uweb4Media.Application.Features.CQRS.Commands.User;

public class CreateUserCommand
{ 
    public string Username { get; set; } 
    public string Password { get; set; } 
    public string Email { get; set; } 
    public string? Name { get; set; }  
    public string? Surname { get; set; } 
    public string? AvatarUrl { get; set; } 
    public string? Bio { get; set; }  
}