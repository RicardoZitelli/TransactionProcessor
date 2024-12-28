namespace TransactionProcessor.Domain.Repositories;

public interface ITransactionRepository
{
    public void SaveTransactionsParallel(IEnumerable<Transaction> transactions, int batchSize = 1000);
    Task<List<UserSummary>> GetUserSummariesAsync();
    Task<List<CategorySummary>> GetTopCategoriesAsync(int topCount);
    Task<HighestSpender?> GetHighestSpenderAsync();
}

public record UserSummary(Guid UserId, decimal TotalIncome, decimal TotalExpense);
public record CategorySummary(string Category, int TransactionsCount);
public record HighestSpender(Guid UserId, decimal TotalExpense);
