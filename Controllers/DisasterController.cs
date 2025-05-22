using Disaster_Prediction_And_Alert_System_API.BusinessLogic.DisasterPredictionAndAlert.Interface;
using Disaster_Prediction_And_Alert_System_API.Model;
using Microsoft.AspNetCore.Mvc;

namespace Disaster_Prediction_And_Alert_System_API.Controllers
{
    [ApiController]
    [Route("api")]
    public class DisasterController : ControllerBase
    {
        IDisasterPredictionAndAlertFacadeService _disasterPredictionAndAlertFacadeService;
        public DisasterController(
            IDisasterPredictionAndAlertFacadeService disasterPredictionAndAlertFacadeService
            )
        {
            _disasterPredictionAndAlertFacadeService = disasterPredictionAndAlertFacadeService;
        }

        [HttpPost("regions")]
        public async Task<IActionResult> AddRegion([FromBody] RegionInfo region)
        {
            try
            {
                var result = await _disasterPredictionAndAlertFacadeService.AddRegion(region);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("alert-settings")]
        public async Task<IActionResult> AddSetting([FromBody] AlertSettingInfo setting)
        {
            try
            {
                var result = await _disasterPredictionAndAlertFacadeService.AddAlertSetting(setting);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("disaster-risks")]
        public async Task<IActionResult> GetRisks()
        {
            try
            {
                var result = await _disasterPredictionAndAlertFacadeService.CalculateRiskAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("alerts/send")]
        public async Task<IActionResult> SendAlerts()
        {
            try
            {
                await _disasterPredictionAndAlertFacadeService.SendAlerts();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("alerts")]
        public async Task<IActionResult> GetAlerts()
        {
            try
            {
                return Ok(await _disasterPredictionAndAlertFacadeService.GetAllAlerts());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
