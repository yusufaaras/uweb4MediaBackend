namespace uweb4Media.Application.Features.CQRS.Commands.Plans;

public class RemovePlansCommand
{
    public  RemovePlansCommand(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}