using uweb4Media.Application.Features.CQRS.Commands.Admin.Sector;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Sector;

public class UpdateSectorCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Admin.Sector.Sector> _repository;

    public UpdateSectorCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Sector.Sector> repository)
    {
        _repository = repository;
    }
    public async Task Handle(UpdateSectorCommand command)
    {
        var Sector = await _repository.GetByIdAsync(command.Id);
        Sector.Name = command.Name;
        await _repository.UpdateAsync(Sector);
        
    }
}