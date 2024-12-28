using CsvHelper;
using CsvHelper.Configuration;
using TransactionProcessor.Application.Interfaces;
using TransactionProcessor.Domain;

namespace TransactionProcessor.Application;
public class TransactionLoader : ITransactionLoader
{
    public IEnumerable<Transaction> LoadFromCsv(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        });

        foreach (var record in csv.GetRecords<Transaction>())
        {
            yield return record;
        }
    }
}