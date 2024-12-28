namespace TransactionProcessor.Domain.Repositories
{
    public interface ICreateDatabaseRepository
    {
        void CreateDatabaseTransactionDB();
        void DropDatabaseTransactionDB();
    }
}