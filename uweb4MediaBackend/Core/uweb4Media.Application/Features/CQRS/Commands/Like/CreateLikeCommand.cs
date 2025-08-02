namespace uweb4Media.Application.Features.CQRS.Commands.Like;

public class CreateLikeCommand
{
    public int UserId { get; set; } 
    public int VideoId { get; set; } 
}