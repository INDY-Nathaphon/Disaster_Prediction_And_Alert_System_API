using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.TransactionManager;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Region.Interface;
using Disaster_Prediction_And_Alert_System_API.Common.Model.Base;
using Disaster_Prediction_And_Alert_System_API.Common.Model.Region;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Region.Facade
{
    public class RegionFacadeService : IRegionFacadeService
    {
        private readonly IRegionService _regionService;
        private readonly ITransactionManagerService _transactionManager;

        public RegionFacadeService(
            IRegionService regionService,
            ITransactionManagerService transactionManager)
        {
            _regionService = regionService;
            _transactionManager = transactionManager;
        }

        public async Task<RegionInfo> GetEntityById(long id)
        {
            return await _transactionManager.DoworkWithNoTransaction(() =>
            _regionService.GetEntityById(id));
        }

        public Task<PagedResult<RegionInfo>> GetEntities(BaseFilter filter)
        {
            return _transactionManager.DoworkWithNoTransaction(() =>
            _regionService.GetEntities(filter));
        }
        public Task<RegionInfo> Create(RegionInfo info)
        {
            return _transactionManager.DoworkWithTransaction(() =>
            _regionService.Create(info));
        }
        public Task<RegionInfo> Update(long id, RegionInfo info)
        {
            return _transactionManager.DoworkWithTransaction(() =>
            _regionService.Update(id, info));
        }
        public async Task Delete(long id)
        {
            await _transactionManager.DoworkWithTransaction(() =>
            _regionService.Delete(id));
        }
    }
}
