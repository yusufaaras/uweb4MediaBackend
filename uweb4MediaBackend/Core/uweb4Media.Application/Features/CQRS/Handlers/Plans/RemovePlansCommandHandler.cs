using uweb4Media.Application.Features.CQRS.Commands.Plans;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Plans;

public class RemovePlansCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Plans> _repository;
    public RemovePlansCommandHandler(IRepository<Uweb4Media.Domain.Entities.Plans> repository)
    {
        _repository = repository;
    }
    public async Task Handle(RemovePlansCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.RemoveAsync(value);
    }
}