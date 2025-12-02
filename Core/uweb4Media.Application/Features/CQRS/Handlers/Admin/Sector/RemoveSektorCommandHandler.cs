using uweb4Media.Application.Features.CQRS.Commands.Admin.Campagin;
using uweb4Media.Application.Features.CQRS.Commands.Admin.Sector;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Sector;

public class RemoveSectorCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Sector.Sector> _repository;

    public RemoveSectorCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Sector.Sector> repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveSectorCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.RemoveAsync(value);
    }
}