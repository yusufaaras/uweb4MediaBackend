namespace uweb4Media.Application.Features.CQRS.Commands.Like;

public class RemoveLikeCommand
{
    public  RemoveLikeCommand(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}