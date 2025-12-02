using uweb4Media.Application.Features.CQRS.Commands.Admin.Sector;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Sector;

public class CreateSectorCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Sector.Sector> _repository; // 'readonly' kullanımı iyi bir pratik

    public CreateSectorCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Sector.Sector> repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreateSectorCommand command)
    {
        await _repository.CreateAsync(new Uweb4Media.Domain.Entities.Admin.Sector.Sector
        {
            Name = command.Name
        });
    }
}