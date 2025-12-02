using MediatR;
using uweb4Media.Application.Features.CQRS.Results.Invoice;

namespace uweb4Media.Application.Features.CQRS.Queries.Invoice;

public class GetInvoicesQuery : IRequest<List<GetInvoicesQueryResult>>
{
    public int UserId { get; set; }

    public GetInvoicesQuery(int userId)
    {
        UserId = userId;
    }
}

