using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.TransactionManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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
    private IDbContextTransaction? _transaction;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        // ถ้ามี transaction ค้างไว้ ให้ rollback ก่อนเปิดใหม่ (ป้องกันหลุด)
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }

        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}

#endregion

public class TransactionManagerService : ITransactionManagerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionManagerService> _logger;

    public TransactionManagerService(ILogger<TransactionManagerService> _logger, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        this._logger = _logger;
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
            _logger.LogError(ex.Message);
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
            _logger.LogError(ex.Message);
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
            _logger.LogError(ex.Message);
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
            _logger.LogError(ex.Message);
            throw;
        }
    }
}
