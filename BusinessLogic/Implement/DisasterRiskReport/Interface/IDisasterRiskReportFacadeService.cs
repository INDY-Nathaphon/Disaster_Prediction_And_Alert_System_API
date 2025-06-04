namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterRiskReport.Interface
{
    public interface IDisasterRiskReportFacadeService
    {
        public Task CreateDisasterRiskReport();

        public Task SendDisasterRiskReport();
    }
}
