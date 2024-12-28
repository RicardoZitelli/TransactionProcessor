using TransactionProcessor.Domain;

namespace TransactionProcessor.Application.Interfaces;
public interface ITransactionLoader
{
    IEnumerable<Transaction> LoadFromCsv(string filePath);
}