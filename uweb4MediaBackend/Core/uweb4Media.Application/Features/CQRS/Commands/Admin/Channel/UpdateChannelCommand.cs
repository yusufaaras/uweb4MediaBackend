namespace uweb4Media.Application.Features.CQRS.Commands.Admin.Channel;

public class UpdateChannelCommand
{
    public int Id { get; set; }
    public string Name { get; set; }
}