namespace uweb4Media.Application.Features.CQRS.Commands.Plans;

public class CreatePlansCommand
{
    public string Name { get; set; }
    public double Price { get; set; }
}