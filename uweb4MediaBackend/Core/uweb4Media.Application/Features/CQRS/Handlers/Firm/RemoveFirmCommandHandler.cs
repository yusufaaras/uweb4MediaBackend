using uweb4Media.Application.Features.CQRS.Commands.Firm;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Firm;

public class RemoveFirmCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Firm> _repository;
    public RemoveFirmCommandHandler(IRepository<Uweb4Media.Domain.Entities.Firm> repository)
    {
        _repository = repository;
    }
    public async Task Handle(RemoveFirmCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.RemoveAsync(value);
    }
}