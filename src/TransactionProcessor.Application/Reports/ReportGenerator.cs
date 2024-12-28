using System.Text.Json;
using TransactionProcessor.Domain.Repositories;

namespace TransactionProcessor.Application.Reports;

public class ReportGenerator
{
    public void GenerateReport(string outputPath, List<UserSummary> userSummaries, List<CategorySummary> topCategories, HighestSpender? highestSpender)
    {
        var report = new
        {
            users_summary = userSummaries,
            top_categories = topCategories,
            highest_spender = highestSpender
        };

        var json = JsonSerializer.Serialize(report,
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(outputPath, json);
    }
}