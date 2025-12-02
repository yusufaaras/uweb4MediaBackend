namespace uweb4Media.Application.Features.CQRS.Commands.Media;

public class RemoveMediaContentCommand
{
    public  RemoveMediaContentCommand(int id)
    {
       Id = id;
    }
    public int Id { get; set; }
}