using uweb4Media.Application.Features.CQRS.Queries.Payment;
using uweb4Media.Application.Features.CQRS.Results.Payment;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Payment
{
    public class GetPaymentsByUserIdQueryHandler
    {
        private readonly IRepository<Uweb4Media.Domain.Entities.Payment> _repository;
        public GetPaymentsByUserIdQueryHandler(IRepository<Uweb4Media.Domain.Entities.Payment> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetPaymentByIdQueryResult>> Handle(GetPaymentsByUserIdQuery query)
        {
            var payments = await _repository.GetAllAsync(p => p.UserId == query.UserId);
            return payments
                .OrderByDescending(p => p.CreatedAt)
                .Select(values => new GetPaymentByIdQueryResult
                {
                    Id = values.Id,
                    Amount = values.Amount,
                    Currency = values.Currency,
                    Status = values.Status,
                    Email = values.Email,
                    UserId = values.UserId,
                    Provider = values.Provider,
                    CreatedAt = values.CreatedAt, 
                    InvoiceId = values.InvoiceId,
                    InvoicePdfUrl = values.InvoicePdfUrl,
                    InvoiceStatus = values.InvoiceStatus
                })
                .ToList();
        }
    }
}