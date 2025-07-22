using System.ComponentModel.DataAnnotations;

namespace Uweb4Media.Domain.Entities.Admin.Sector;

public class Sector
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
}