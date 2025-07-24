using Microsoft.EntityFrameworkCore;
using uweb4Media.Application.Interfaces.Payment;
using Uweb4Media.Persistence.Context;

namespace Uweb4Media.Persistence.Repositories.Payment;

public class PaymentRepository : Repository<Domain.Entities.Payment>, IPaymentRepository
{
    private readonly Uweb4MediaContext _context;

    public PaymentRepository(Uweb4MediaContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Payment?> GetByOrderIdAsync(string orderId)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }
}