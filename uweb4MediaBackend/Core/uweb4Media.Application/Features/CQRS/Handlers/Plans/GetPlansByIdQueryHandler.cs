using uweb4Media.Application.Features.CQRS.Queries.Plans;
using uweb4Media.Application.Features.CQRS.Results.Plans;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Plans;

public class GetPlansByIdQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Plans> _repository;
    public GetPlansByIdQueryHandler(IRepository<Uweb4Media.Domain.Entities.Plans> repository)
    {
        _repository = repository;
    }
    public async Task<GetPlansByIdQueryResult> Handle(GetPlansByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetPlansByIdQueryResult
        {
            Id = values.Id,
            Name = values.Name, 
            Price = values.Price,
            status = values.status,
        };
    }
}