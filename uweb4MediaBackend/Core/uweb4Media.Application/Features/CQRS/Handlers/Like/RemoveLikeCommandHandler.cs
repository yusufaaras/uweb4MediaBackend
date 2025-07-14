using uweb4Media.Application.Features.CQRS.Commands.Like;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Like;

public class RemoveLikeCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Like> _repository;
    public RemoveLikeCommandHandler(IRepository<Uweb4Media.Domain.Entities.Like> repository)
    {
        _repository = repository;
    }
    public async Task Handle(RemoveLikeCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.RemoveAsync(value);
    }
}