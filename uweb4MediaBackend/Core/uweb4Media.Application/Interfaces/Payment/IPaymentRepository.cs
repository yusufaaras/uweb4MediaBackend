namespace uweb4Media.Application.Interfaces.Payment;
 
public interface IPaymentRepository : IRepository<Uweb4Media.Domain.Entities.Payment>
{
    Task<Uweb4Media.Domain.Entities.Payment?> GetByOrderIdAsync(string orderId);
    Task<Uweb4Media.Domain.Entities.Payment?> GetByStripePaymentIntentIdAsync(string paymentIntentId);

}