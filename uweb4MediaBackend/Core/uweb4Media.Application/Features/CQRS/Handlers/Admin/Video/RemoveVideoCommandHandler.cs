using uweb4Media.Application.Features.CQRS.Commands.Admin.Video;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;

public class RemoveVideoCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> _repository;
    public RemoveVideoCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> repository)
    {
        _repository = repository;
    }
    public async Task Handle(RemoveVideoCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.RemoveAsync(value);
    }
}