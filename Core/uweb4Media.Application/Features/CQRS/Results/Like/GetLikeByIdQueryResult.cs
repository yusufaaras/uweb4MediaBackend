namespace uweb4Media.Application.Features.CQRS.Results.Like;

public class GetLikeByIdQueryResult
{
    public int Id { get; set; } 
    public int UserId { get; set; } 
    public int VideoId { get; set; } 
    public DateTime LikeDate { get; set; } 
    
}