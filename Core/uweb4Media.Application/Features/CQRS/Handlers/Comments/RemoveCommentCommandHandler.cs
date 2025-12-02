using uweb4Media.Application.Features.CQRS.Commands.Comments;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.Comments;

public class RemoveCommentCommandHandler
{
    private readonly IRepository<Comment> _repository;
    public RemoveCommentCommandHandler(IRepository<Comment> repository)
    {
        _repository = repository;
    }
    public async Task Handle(RemoveCommentCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.RemoveAsync(value);
    }
}