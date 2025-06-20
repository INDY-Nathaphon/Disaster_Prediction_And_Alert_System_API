using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.TransactionManager;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterPredictionAndAlert.Interface;
using Disaster_Prediction_And_Alert_System_API.Common.Models.AlertSetting;
using Disaster_Prediction_And_Alert_System_API.Common.Models.Base;

public class AlertSettingFacadeService : IAlertSettingFacadeService
{
    private readonly IAlertSettingService _alertSettingService;
    private readonly ITransactionManagerService _transactionManager;

    public AlertSettingFacadeService(
        IAlertSettingService _alertSettingService,
        ITransactionManagerService transactionManager)
    {
        this._alertSettingService = _alertSettingService;
        _transactionManager = transactionManager;
    }

    public async Task<AlertSettingInfo> GetEntityById(long id)
    {
        return await _transactionManager.DoworkWithNoTransaction(() =>
        _alertSettingService.GetEntityById(id));
    }

    public Task<PagedResult<AlertSettingInfo>> GetEntities(BaseFilter filter)
    {
        return _transactionManager.DoworkWithNoTransaction(() =>
        _alertSettingService.GetEntities(filter));
    }
    public Task<AlertSettingInfo> Create(AlertSettingInfo info)
    {
        return _transactionManager.DoworkWithTransaction(() =>
        _alertSettingService.Create(info));
    }
    public Task<AlertSettingInfo> Update(long id, AlertSettingInfo info)
    {
        return _transactionManager.DoworkWithTransaction(() =>
        _alertSettingService.Update(id, info));
    }
    public async Task Delete(long id)
    {
        await _transactionManager.DoworkWithTransaction(() =>
        _alertSettingService.Delete(id));
    }
}
