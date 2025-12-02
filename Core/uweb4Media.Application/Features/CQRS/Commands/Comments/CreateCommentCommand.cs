namespace uweb4Media.Application.Features.CQRS.Commands.Comments;

public class CreateCommentCommand
{
    public string Text { get; set; } 
    public int UserId { get; set; } 
    public int VideoId { get; set; } 
}