using Disaster_Prediction_And_Alert_System_API.Common.Model.Base;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Base
{
    public interface IBaseService<T, TFilter>
    where TFilter : BaseFilter
    where T : class
    {
        Task<T> GetEntityById(long id);
        Task<PagedResult<T>> GetEntities(TFilter filter);
        Task<T> Create(T entity);
        Task<T> Update(long id, T entity);
        Task Delete(long id);
    }
}
