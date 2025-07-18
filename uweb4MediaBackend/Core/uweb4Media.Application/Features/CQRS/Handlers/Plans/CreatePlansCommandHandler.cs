using uweb4Media.Application.Features.CQRS.Commands.Plans;
using uweb4Media.Application.Interfaces; 
namespace uweb4Media.Application.Features.CQRS.Handlers.Plans;

public class CreatePlansCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Plans> _repository;

    public CreatePlansCommandHandler(IRepository<Uweb4Media.Domain.Entities.Plans> repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreatePlansCommand command)
    {
        await _repository.CreateAsync(new Uweb4Media.Domain.Entities.Plans
        {
            Name = command.Name,
            Price = command.Price,
            status = false 
        });

    }
}