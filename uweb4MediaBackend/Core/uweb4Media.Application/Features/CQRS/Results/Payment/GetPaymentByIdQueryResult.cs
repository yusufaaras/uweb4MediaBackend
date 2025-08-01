namespace uweb4Media.Application.Features.CQRS.Results.Payment;

public class GetPaymentByIdQueryResult
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }  
    public string Email { get; set; }
    public int UserId { get; set; }
    public string? Provider { get; set; } // "iyzico", "stripe" gibi
    public DateTime CreatedAt { get; set; }
    public string? InvoiceId { get; set; }
    public string? InvoicePdfUrl { get; set; }
    public string? InvoiceStatus { get; set; }
    
}