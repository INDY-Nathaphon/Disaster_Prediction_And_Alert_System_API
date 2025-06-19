namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.TransactionManager
{
    public interface ITransactionManagerService
    {
        Task<TResult> DoworkWithTransaction<TResult>(Func<Task<TResult>> operation);
        Task<TResult> DoworkWithNoTransaction<TResult>(Func<Task<TResult>> operation);
        Task DoworkWithTransaction(Func<Task> operation);
        Task DoworkWithNoTransaction(Func<Task> operation);
    }
}
