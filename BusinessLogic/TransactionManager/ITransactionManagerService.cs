namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.TransactionManager
{
    public interface ITransactionManagerService
    {
        Task<TResult> DoworkWithTransaction<TResult>(Func<Task<TResult>> operation);
        Task<TResult> DoworkWithNoTransaction<TResult>(Func<Task<TResult>> operation);
    }
}
