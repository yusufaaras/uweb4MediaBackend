namespace Uweb4Media.Domain.Entities.StripePayment;

public class PartnerShare
{
    public int Id { get; set; }
    public string StripeAccountId { get; set; }
    public decimal Percentage { get; set; }
}