namespace Uweb4Media.Domain.Entities;

public class Plans
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public bool status { get; set; }=false; 
}