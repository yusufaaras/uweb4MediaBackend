namespace uweb4Media.Application.Features.CQRS.Queries.Payment
{
    public class GetPaymentsByUserIdQuery
    {
        public GetPaymentsByUserIdQuery(int userId)
        {
            UserId = userId;
        }
        public int UserId { get; set; }
    }
}