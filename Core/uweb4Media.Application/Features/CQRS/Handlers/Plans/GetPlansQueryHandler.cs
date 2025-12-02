using uweb4Media.Application.Features.CQRS.Results.Plans;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Plans;

public class GetPlansQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Plans> _repository;

    public GetPlansQueryHandler(IRepository<Uweb4Media.Domain.Entities.Plans> repository)
    {
        _repository = repository;
    }
    public async Task<List<GetPlansQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetPlansQueryResult
        {
            Id = x.Id,
            Price = x.Price,
            Description = x.Description,
            Icon = x.Icon,
            Name = x.Name,
            status = x.status,
            IsToken = x.IsToken,
            TokenCount = x.TokenCount // <-- EKLE
        }).ToList();
    }
}