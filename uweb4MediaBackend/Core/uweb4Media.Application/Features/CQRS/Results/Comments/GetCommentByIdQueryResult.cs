namespace uweb4Media.Application.Features.CQRS.Results.Comments;

public class GetCommentByIdQueryResult
{
    public int Id { get; set; } 
    public string Text { get; set; } 
    public DateTime CommentDate { get; set; } 
    public int UserId { get; set; }  
    public int MediaContentId { get; set; }  
    
}