using TransactionProcessor.Application;
using TransactionProcessor.Infrastructure;
using TransactionProcessor.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TransactionProcessor.Application.Interfaces;
using TransactionProcessor.Application.Reports;
using TransactionProcessor.Infrastructure.Interfaces;
using TransactionProcessor.Domain.Utilities;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace TransactionProcessor.App;

public class Program
{
    public static async Task Main(string[] args)
    {

        ILogger? logger = null;

        try
        {
            const string csvPath = "transactions.csv";
            const string reportPath = "Reports/report.json";

            var hostCreateDatabase = CreateHost(args, "InitialConnection");
            CreateDatabaseIfNotExists(hostCreateDatabase);

            var host = CreateHost(args, "DefaultConnection");
            logger = host.Services.GetRequiredService<ILogger<Program>>();

            CreateTableIfNotExists(host);

            await ProcessTransactionsAsync(host, csvPath, reportPath);

            logger.LogInformation("Application executed successfully.");
        }
        catch (Exception ex)
        {
            if (logger == null)
            {
                Console.Error.WriteLine($"An error occurred during initialization: {ex.Message}");
                Console.Error.WriteLine(ex.StackTrace);
            }
            else
            {
                logger.LogError(ex, "An error occurred during execution.");
            }
        }
    }

    private static IHost CreateHost(string[] args, string connectionName)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                services.AddSingleton<IDatabaseConnection>(_ =>
                {
                    var connectionString = GetConnectionString(configuration, connectionName);
                    return new DatabaseConnection(connectionString);
                });

                if (connectionName == "InitialConnection")
                {
                    services.AddSingleton<ICreateDatabaseRepository, CreateDatabaseRepository>();
                }
                else
                {
                    services.AddSingleton<ICreateTableRepository, CreateTableRepository>();
                    services.AddSingleton<ITransactionLoader, TransactionLoader>();
                    services.AddSingleton<ITransactionRepository, TransactionRepository>();
                    services.AddSingleton<ReportGenerator>();
                }

                services.AddLogging();
            })
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .Build();
    }

    private static string GetConnectionString(IConfiguration configuration, string connectionName)
    {
        var connectionString = configuration.GetConnectionString(connectionName);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{connectionName}' not found.");
        }
        return connectionString;
    }

    private static void CreateDatabaseIfNotExists(IHost hostCreateDatabase)
    {
        var repositoryDatabase = hostCreateDatabase.Services.GetRequiredService<ICreateDatabaseRepository>();

        repositoryDatabase.DropDatabaseTransactionDB();
        repositoryDatabase.CreateDatabaseTransactionDB();
    }

    private static void CreateTableIfNotExists(IHost hostCreateDatabase)
    {
        var repositoryTable = hostCreateDatabase.Services.GetRequiredService<ICreateTableRepository>();

        repositoryTable.DropTableInTransactionDB();
        repositoryTable.CreateTableInTransactionDB();
    }

    private static async Task ProcessTransactionsAsync(IHost host, string csvPath, string reportPath)
    {
        var transactionLoader = host.Services.GetRequiredService<ITransactionLoader>();
        var repository = host.Services.GetRequiredService<ITransactionRepository>();
        var reportGenerator = host.Services.GetRequiredService<ReportGenerator>();
        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        var stopwatch = Stopwatch.StartNew();

        var transactions = transactionLoader.LoadFromCsv(csvPath);
        repository.SaveTransactionsParallel(transactions);

        var userSummaries = await repository.GetUserSummariesAsync();
        var topCategories = await repository.GetTopCategoriesAsync(3);
        var highestSpender = await repository.GetHighestSpenderAsync();

        var newNameReport = Common.GetNextReportFileName(reportPath);
        reportGenerator.GenerateReport(newNameReport, userSummaries, topCategories, highestSpender);

        stopwatch.Stop();
        logger.LogInformation($"Execution time: {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
    }
}