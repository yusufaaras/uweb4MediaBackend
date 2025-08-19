namespace uweb4Media.Application.Features.CQRS.Commands.Plans;

public class UpdatePlansCommand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public double Price { get; set; }
    public bool status { get; set; }
    public bool IsToken { get; set; }
    public int? TokenCount { get; set; } // <-- EKLE
}