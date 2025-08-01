namespace Uweb4Media.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public string OrderId { get; set; }
    public string? IyzicoPaymentId { get; set; }
    public string? StripePaymentIntentId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
    public string Email { get; set; }
    public int UserId { get; set; }
    public string? Provider { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? PaymentCode { get; set; } // yeni alan
    public DateTime? PaymentCodeGeneratedAt { get; set; }
    public string? InvoiceId { get; set; }
    public string? InvoicePdfUrl { get; set; }
    public string? InvoiceStatus { get; set; }
}