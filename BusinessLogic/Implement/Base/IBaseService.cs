namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Base
{
    public interface IBaseService<T> where T : class
    {
        Task<T> GetById(long id);
        Task<List<T>> GetAll();
        Task<T> Create(T entity);
        Task<T> Update(long id, T entity);
        Task Delete(long id);
    }
}
