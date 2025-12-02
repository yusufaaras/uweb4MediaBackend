using System.ComponentModel.DataAnnotations;

namespace Uweb4Media.Domain.Entities.Admin.Channel;

public class Channel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
}