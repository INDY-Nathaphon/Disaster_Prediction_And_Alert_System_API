using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterPredictionAndAlert.Interface;
using Disaster_Prediction_And_Alert_System_API.Common.ApiResponse;
using Disaster_Prediction_And_Alert_System_API.Common.Model.AlertSetting;
using Disaster_Prediction_And_Alert_System_API.Common.Model.Base;
using Microsoft.AspNetCore.Mvc;

namespace Disaster_Prediction_And_Alert_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertSettingController : ControllerBase
    {
        private readonly IAlertSettingFacadeService _alertSettingFacadeService;

        public AlertSettingController(IAlertSettingFacadeService alertSettingFacadeService)
        {
            _alertSettingFacadeService = alertSettingFacadeService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<AlertSettingInfo>>>> GetEntities([FromQuery] BaseFilter filter)
        {
            var result = await _alertSettingFacadeService.GetEntities(filter);
            return Ok(ApiResponseFactory.Success(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AlertSettingInfo>>> GetEntityById(long id)
        {
            var result = await _alertSettingFacadeService.GetEntityById(id);
            return Ok(ApiResponseFactory.Success(result));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AlertSettingInfo>>> Create([FromBody] AlertSettingInfo info)
        {
            var result = await _alertSettingFacadeService.Create(info);
            return Ok(ApiResponseFactory.Success(result));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<AlertSettingInfo>>> Update(long id, [FromBody] AlertSettingInfo info)
        {
            var result = await _alertSettingFacadeService.Update(id, info);
            return Ok(ApiResponseFactory.Success(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _alertSettingFacadeService.Delete(id);
            return NoContent();
        }
    }
}
