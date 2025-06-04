using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.TransactionManager;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Region.Interface;
using Disaster_Prediction_And_Alert_System_API.Model;

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

        public async Task<RegionInfo> GetById(long id)
        {
            return await _transactionManager.DoworkWithNoTransaction(() =>
            _regionService.GetById(id));
        }

        public Task<List<RegionInfo>> GetAll()
        {
            return _transactionManager.DoworkWithNoTransaction(() =>
            _regionService.GetAll());
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
