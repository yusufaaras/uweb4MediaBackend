namespace uweb4Media.Application.Features.CQRS.Results.Invoice;

public class GetInvoicesQueryResult
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }  
    public string Email { get; set; }  
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } 
    public string? InvoicePdfUrl { get; set; } 
}