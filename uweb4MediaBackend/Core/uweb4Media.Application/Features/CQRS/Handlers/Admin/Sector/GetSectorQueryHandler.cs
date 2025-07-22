using uweb4Media.Application.Features.CQRS.Results.Admin.Sector;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Sector;

public class GetSectorQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Sector.Sector> _repository;

    public GetSectorQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Sector.Sector> repository)
    {
        _repository = repository;
    }
    public async Task<List<GetSectorQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetSectorQueryResult
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }
}