namespace uweb4Media.Application.Features.CQRS.Commands.Comments;

public class RemoveCommentCommand
{
    public  RemoveCommentCommand(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}
