namespace TransactionProcessor.Domain;

public class Transaction
{
    public Guid TransactionId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? Merchant { get; set; }
}
