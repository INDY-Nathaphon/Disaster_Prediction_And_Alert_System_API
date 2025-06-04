using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.TransactionManager;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterRiskReport.Interface;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterRiskReport.Faacade
{
    public class DisasterRiskReportFacadeService : IDisasterRiskReportFacadeService
    {
        private readonly IDisasterRiskReportService _disasterRiskReportService;
        private readonly ITransactionManagerService _transactionManager;

        public DisasterRiskReportFacadeService(
            IDisasterRiskReportService disasterRiskReportService,
            ITransactionManagerService transactionManager)
        {
            _disasterRiskReportService = disasterRiskReportService;
            _transactionManager = transactionManager;
        }

        public async Task CreateDisasterRiskReport()
        {
            await _transactionManager.DoworkWithTransaction(() =>
            _disasterRiskReportService.CreateDisasterRiskReport());
        }

        public async Task SendDisasterRiskReport()
        {
            await _transactionManager.DoworkWithTransaction(() =>
            _disasterRiskReportService.SendDisasterRiskReport());
        }
    }
}
