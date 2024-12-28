namespace TransactionProcessor.Domain.Repositories
{
    public interface ICreateTableRepository
    {
        void CreateTableInTransactionDB();
        void DropTableInTransactionDB();
    }
}