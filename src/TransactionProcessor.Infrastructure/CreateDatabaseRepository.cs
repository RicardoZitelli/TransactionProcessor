using Dapper;
using TransactionProcessor.Domain.Repositories;
using TransactionProcessor.Infrastructure.Interfaces;

namespace TransactionProcessor.Infrastructure;

public class CreateDatabaseRepository : ICreateDatabaseRepository
{
    private readonly IDatabaseConnection _databaseConnection;

    public CreateDatabaseRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }

    public void DropDatabaseTransactionDB()
    {
        const string dropQuery = @"
            IF EXISTS (SELECT name FROM sys.databases WHERE name = 'TransactionDB')
            BEGIN
                DROP DATABASE TransactionDB
            END";

        using var connection = _databaseConnection.CreateConnection();
        connection.Open();

        connection.Execute(dropQuery);
    }

    public void CreateDatabaseTransactionDB()
    {
        const string createQuery = @"
            CREATE DATABASE TransactionDB";

        using var connection = _databaseConnection.CreateConnection();
        connection.Open();

        connection.Execute(createQuery);
    }
}
