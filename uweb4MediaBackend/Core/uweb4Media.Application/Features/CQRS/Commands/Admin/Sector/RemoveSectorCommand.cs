namespace uweb4Media.Application.Features.CQRS.Commands.Admin.Sector;

public class RemoveSectorCommand
{
    public int Id { get; set; }

    public RemoveSectorCommand(int id)
    {
        Id = id;
    }
}