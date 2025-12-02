using uweb4Media.Application.Features.CQRS.Commands.Media;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.Media;

public class RemoveMediaContentCommandHandler
{
    private readonly IRepository<MediaContent> _repository;
    public RemoveMediaContentCommandHandler(IRepository<MediaContent> repository)
    {
        _repository = repository;
    }
    public async Task Handle(RemoveMediaContentCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.RemoveAsync(value);
    }
}