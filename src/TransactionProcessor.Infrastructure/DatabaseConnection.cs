using System.Data;
using Microsoft.Data.SqlClient;
using TransactionProcessor.Infrastructure.Interfaces;

namespace TransactionProcessor.Infrastructure;

public class DatabaseConnection : IDatabaseConnection
{
    private readonly string _connectionString;

    public DatabaseConnection(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));
        }

        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        try
        {
            return new SqlConnection(_connectionString);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to create database connection", ex);
        }
    }
}
