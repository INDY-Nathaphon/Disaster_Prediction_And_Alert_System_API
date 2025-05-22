using Disaster_Prediction_And_Alert_System_API.BusinessLogic.DisasterPredictionAndAlert.Interface;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.TransactionManager;
using Disaster_Prediction_And_Alert_System_API.Domain.Entities;
using Disaster_Prediction_And_Alert_System_API.Model;

public class DisasterPredictionAndAlertFacadeService : IDisasterPredictionAndAlertFacadeService
{
    private readonly IDisasterPredictionAndAlertService _disasterPredictionAndAlertService;
    private readonly ITransactionManagerService _transactionManager;

    public DisasterPredictionAndAlertFacadeService(
        IDisasterPredictionAndAlertService disasterPredictionAndAlertService,
        ITransactionManagerService transactionManager)
    {
        _disasterPredictionAndAlertService = disasterPredictionAndAlertService;
        _transactionManager = transactionManager;
    }

    public async Task<Region> AddRegion(RegionInfo region)
    {
        return await _transactionManager.DoworkWithTransaction(() =>
            _disasterPredictionAndAlertService.AddRegion(region));
    }

    public async Task<AlertSetting> AddAlertSetting(AlertSettingInfo setting)
    {
        return await _transactionManager.DoworkWithTransaction(() =>
            _disasterPredictionAndAlertService.AddAlertSetting(setting));
    }

    public async Task SendAlerts()
    {
        await _disasterPredictionAndAlertService.SendAlerts();
    }

    public async Task<List<Alert>> GetAllAlerts()
    {
        return await _transactionManager.DoworkWithNoTransaction(() =>
            _disasterPredictionAndAlertService.GetAllAlerts());
    }

    public async Task<List<DisasterRiskReport>> CalculateRiskAsync()
    {
        return await _transactionManager.DoworkWithTransaction(() =>
            _disasterPredictionAndAlertService.CalculateRiskAsync());
    }
}
