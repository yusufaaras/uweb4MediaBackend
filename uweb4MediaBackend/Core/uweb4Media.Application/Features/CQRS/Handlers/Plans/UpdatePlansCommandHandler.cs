using uweb4Media.Application.Features.CQRS.Commands.Plans;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Plans;

public class UpdatePlansCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Plans> _repository;

    public UpdatePlansCommandHandler(IRepository<Uweb4Media.Domain.Entities.Plans> repository)
    {
        _repository = repository;
    }
    public async Task Handle(UpdatePlansCommand command)
    {
        var values = await _repository.GetByIdAsync(command.Id);
        values.Name = command.Name;
        values.Price = command.Price;
        values.status=command.status;
        await _repository.UpdateAsync(values);
    }
}