using uweb4Media.Application.Features.CQRS.Queries.Admin.Channel;
using uweb4Media.Application.Features.CQRS.Results.Admin.Channel;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Channel;

public class GetChannelByIdQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Channel.Channel> _repository;
    public GetChannelByIdQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Channel.Channel> repository)
    {
        _repository = repository;
    }
    public async Task<GetChannelByIdQueryResult> Handle(GetChannelByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetChannelByIdQueryResult
        {
            Id = values.Id,
            Name = values.Name
        };
    }
}