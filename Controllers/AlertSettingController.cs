using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterPredictionAndAlert.Interface;
using Disaster_Prediction_And_Alert_System_API.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Disaster_Prediction_And_Alert_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertSettingController : ControllerBase
    {
        private readonly IAlertSettingFacadeService _alertSettingFacadeService;
        private readonly ILogger<UserController> _logger;

        public AlertSettingController(ILogger<UserController> logger, IAlertSettingFacadeService alertSettingFacadeService)
        {
            _alertSettingFacadeService = alertSettingFacadeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlertSettingInfo>>> GetAll()
        {
            try
            {
                var result = await _alertSettingFacadeService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlertSettingInfo>> GetById(long id)
        {
            try
            {
                var result = await _alertSettingFacadeService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<AlertSettingInfo>> Create([FromBody] AlertSettingInfo info)
        {
            try
            {
                var created = await _alertSettingFacadeService.Create(info);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AlertSettingInfo>> Update(long id, [FromBody] AlertSettingInfo info)
        {
            try
            {
                var updated = await _alertSettingFacadeService.Update(id, info);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _alertSettingFacadeService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
