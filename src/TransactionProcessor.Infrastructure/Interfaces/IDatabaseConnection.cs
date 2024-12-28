using System.Data;

namespace TransactionProcessor.Infrastructure.Interfaces;

public interface IDatabaseConnection
{
    IDbConnection CreateConnection();
}