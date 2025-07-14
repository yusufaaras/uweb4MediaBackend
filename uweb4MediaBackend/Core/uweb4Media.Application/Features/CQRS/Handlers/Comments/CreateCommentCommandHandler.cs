using uweb4Media.Application.Features.CQRS.Commands.Comments;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.Comments;

public class CreateCommentCommandHandler
{
    private IRepository<Comment> _repository;

    public CreateCommentCommandHandler(IRepository<Comment> repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreateCommentCommand command)
    {
        await _repository.CreateAsync(new Comment
        { 
            UserId = command.UserId,
            Text = command.Text,    
            MediaContentId = command.MediaContentId,
            CommentDate = command.CommentDate,
        });
    }
    
}