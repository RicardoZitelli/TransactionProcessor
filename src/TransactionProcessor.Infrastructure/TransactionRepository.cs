using Dapper;
using TransactionProcessor.Domain;
using TransactionProcessor.Domain.Repositories;
using TransactionProcessor.Infrastructure.Interfaces;

public class TransactionRepository : ITransactionRepository
{
    private readonly IDatabaseConnection _databaseConnection;

    public TransactionRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }

    public void SaveTransactionsParallel(IEnumerable<Transaction> transactions, int batchSize = 10000)
    {
        var batches = transactions
            .Select((transaction, index) => new { transaction, index })
            .GroupBy(x => x.index / batchSize)
            .Select(g => g.Select(x => x.transaction).ToList())
            .ToList();

        int totalBatches = batches.Count;
        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        Parallel.ForEach(batches.Select((batch, index) => new { batch, index }),
            parallelOptions,
            batchInfo =>
            {
                Console.WriteLine($"Processing batch {batchInfo.index + 1} of {totalBatches} with {batchInfo.batch.Count} transactions...");
                SaveBatch(batchInfo.batch);
                Console.WriteLine($"Batch {batchInfo.index + 1} of {totalBatches} processed successfully.");
            });
    }

    private void SaveBatch(List<Transaction> batch)
    {
        const string insertQuery = @"
            INSERT INTO Transactions 
            (TransactionId, UserId, Date, Amount, Category, Description, Merchant)
            VALUES 
            (@TransactionId, @UserId, @Date, @Amount, @Category, @Description, @Merchant)";

        using var connection = _databaseConnection.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();
        connection.Execute(insertQuery, batch, transaction: transaction);
        transaction.Commit();
    }

    public async Task<List<UserSummary>> GetUserSummariesAsync()
    {
        const string query = @"
            SELECT 
                UserId, 
                SUM(CASE WHEN Amount > 0 THEN Amount ELSE 0 END) AS TotalIncome, 
                SUM(CASE WHEN Amount < 0 THEN Amount ELSE 0 END) AS TotalExpense
            FROM Transactions
            GROUP BY UserId";

        using var connection = _databaseConnection.CreateConnection();
        var result = await connection.QueryAsync<UserSummary>(query);
        return result.ToList();
    }

    public async Task<List<CategorySummary>> GetTopCategoriesAsync(int topCount)
    {
        const string query = @"
            SELECT TOP (@TopCount) 
                Category, 
                COUNT(*) AS TransactionsCount
            FROM Transactions
            GROUP BY Category
            ORDER BY TransactionsCount DESC";

        using var connection = _databaseConnection.CreateConnection();
        var result = await connection.QueryAsync<CategorySummary>(query, new { TopCount = topCount });
        return result.ToList();
    }

    public async Task<HighestSpender?> GetHighestSpenderAsync()
    {
        const string query = @"
            SELECT TOP 1 
                UserId, 
                SUM(Amount) AS TotalExpense
            FROM Transactions
            WHERE Amount < 0
            GROUP BY UserId
            ORDER BY TotalExpense ASC";

        using var connection = _databaseConnection.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<HighestSpender>(query);
        return result;
    }
}