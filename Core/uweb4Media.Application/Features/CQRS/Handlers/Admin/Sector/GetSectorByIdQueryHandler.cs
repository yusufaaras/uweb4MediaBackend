using uweb4Media.Application.Features.CQRS.Queries.Admin.Sector;
using uweb4Media.Application.Features.CQRS.Results.Admin.Sector;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Sector;

public class GetSectorByIdQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Sector.Sector> _repository;
    public GetSectorByIdQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Sector.Sector> repository)
    {
        _repository = repository;
    }
    public async Task<GetSectorByIdQueryResult> Handle(GetSectorByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetSectorByIdQueryResult
        {
            Id = values.Id,
            Name = values.Name
        };
    }
}