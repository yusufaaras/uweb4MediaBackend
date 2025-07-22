namespace uweb4Media.Application.Features.CQRS.Commands.Admin.Channel;

public class RemoveChannelCommand
{
    public int Id { get; set; }

    public RemoveChannelCommand(int id)
    {
        Id = id;
    }
}