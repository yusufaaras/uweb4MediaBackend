namespace uweb4Media.Application.Features.CQRS.Commands.Admin.Campagin;

public class RemoveCampaginCommand
{
    public int Id { get; set; }

    public RemoveCampaginCommand(int id)
    {
        Id = id;
    }
} 