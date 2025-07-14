namespace uweb4Media.Application.Features.CQRS.Commands.Like;

public class CreateLikeCommand
{
    public int Id { get; set; } 
    public int UserId { get; set; } 
    public int MediaContentId { get; set; } 
    public DateTime LikeDate { get; set; } = DateTime.UtcNow;
}