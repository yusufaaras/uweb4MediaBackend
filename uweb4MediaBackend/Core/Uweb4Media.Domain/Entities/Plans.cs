namespace Uweb4Media.Domain.Entities;

public class Plans
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public double Price { get; set; }
    public bool status { get; set; } = false;
    public bool IsToken { get; set; }
    public int? TokenCount { get; set; } 
}