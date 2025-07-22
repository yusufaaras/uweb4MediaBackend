using uweb4Media.Application.Features.CQRS.Results.Admin.Channel;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Channel;

public class GetChannelQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Channel.Channel> _repository;

    public GetChannelQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Channel.Channel> repository)
    {
        _repository = repository;
    }
    public async Task<List<GetChannelQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetChannelQueryResult
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }
}