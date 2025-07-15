namespace uweb4Media.Application.Features.CQRS.Commands.Comments;

public class CreateCommentCommand
{
    public string Text { get; set; } 
    public DateTime CommentDate { get; set; } = DateTime.UtcNow; 
    public int UserId { get; set; } 
    public int MediaContentId { get; set; } 
}