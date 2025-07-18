namespace uweb4Media.Application.Features.CQRS.Commands.Firm;

public class RemoveFirmCommand
{
    public  RemoveFirmCommand(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}