namespace Uweb4Media.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public string OrderId { get; set; }
    public string? IyzicoPaymentId { get; set; }// iyzico'dan dönen ödeme kimliği
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; } // "success", "failure" vb.
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}