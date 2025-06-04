using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.TransactionManager;
using Microsoft.EntityFrameworkCore;

#region UnitOfWork

public interface IUnitOfWork
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
}

#endregion

public class TransactionManagerService : ITransactionManagerService
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionManagerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResult> DoworkWithTransaction<TResult>(Func<Task<TResult>> operation)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            TResult result = await operation.Invoke();
            await _unitOfWork.CommitTransactionAsync();
            return result;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<TResult> DoworkWithNoTransaction<TResult>(Func<Task<TResult>> operation)
    {
        try
        {
            return await operation.Invoke();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public async Task DoworkWithTransaction(Func<Task> operation)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await operation.Invoke();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task DoworkWithNoTransaction(Func<Task> operation)
    {
        try
        {
            await operation.Invoke();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
