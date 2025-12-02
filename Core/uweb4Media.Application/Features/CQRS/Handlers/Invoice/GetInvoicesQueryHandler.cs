using MediatR;
using uweb4Media.Application.Features.CQRS.Queries.Invoice;
using uweb4Media.Application.Features.CQRS.Results.Invoice;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Invoice;

public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, List<GetInvoicesQueryResult>>
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Payment> _repository;

    public GetInvoicesQueryHandler(IRepository<Uweb4Media.Domain.Entities.Payment> repository)
    {
        _repository = repository;
    }

    public async Task<List<GetInvoicesQueryResult>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        var values = await _repository.GetAllAsync();

        var filteredAndSorted = values
            .Where(x => x.UserId == request.UserId)
            .OrderBy(x => x.CreatedAt)
            .ToList();

        return filteredAndSorted.Select(x => new GetInvoicesQueryResult
        {
            Id = x.Id,
            Amount = x.Amount,
            Currency = x.Currency,
            Status = x.Status,
            Email = x.Email,
            UserId = x.UserId,
            CreatedAt = x.CreatedAt,
            InvoicePdfUrl = x.InvoicePdfUrl
        }).ToList();
    }
}
