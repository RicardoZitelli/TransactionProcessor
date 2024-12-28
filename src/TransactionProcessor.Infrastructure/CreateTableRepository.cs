using Dapper;
using TransactionProcessor.Domain.Repositories;
using TransactionProcessor.Infrastructure.Interfaces;

namespace TransactionProcessor.Infrastructure;

public class CreateTableRepository : ICreateTableRepository
{

    private readonly IDatabaseConnection _databaseConnection;

    public CreateTableRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }

    public void DropTableInTransactionDB()
    {
        const string createTableQuery = @"
            IF OBJECT_ID('dbo.Transactions', 'U') IS NOT NULL
            BEGIN
                DROP TABLE dbo.Transactions
            END"
        ;

        using var connection = _databaseConnection.CreateConnection();
        connection.Open();

        connection.Execute(createTableQuery);
    }

    public void CreateTableInTransactionDB()
    {
        const string createTableQuery = @"
            CREATE TABLE Transactions (
                TransactionId UNIQUEIDENTIFIER NOT NULL,
                UserId UNIQUEIDENTIFIER NOT NULL,
                Date DATETIME NOT NULL,
                Amount DECIMAL(18, 2) NOT NULL,
                Category NVARCHAR(255) NOT NULL,
                Description NVARCHAR(MAX),
                Merchant NVARCHAR(255),
                PRIMARY KEY (TransactionId)
            )"
        ;

        using var connection = _databaseConnection.CreateConnection();
        connection.Open();

        connection.Execute(createTableQuery);
    }
}