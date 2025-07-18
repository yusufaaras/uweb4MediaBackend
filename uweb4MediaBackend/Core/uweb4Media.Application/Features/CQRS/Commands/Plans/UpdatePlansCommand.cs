namespace uweb4Media.Application.Features.CQRS.Commands.Plans;

public class UpdatePlansCommand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public bool status { get; set; }
}