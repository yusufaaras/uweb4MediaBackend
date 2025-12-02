namespace uweb4Media.Application.Features.CQRS.Commands.Admin.Video;

public class RemoveVideoCommand
{
    public int Id { get; set; }

    public RemoveVideoCommand(int id)
    {
        Id = id;
    }
}