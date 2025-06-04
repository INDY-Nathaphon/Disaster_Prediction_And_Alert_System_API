using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterPredictionAndAlert.Interface;
using Disaster_Prediction_And_Alert_System_API.Model;
using Microsoft.AspNetCore.Mvc;

namespace Disaster_Prediction_And_Alert_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertSettingController : ControllerBase
    {
        IAlertSettingFacadeService _alertSettingFacadeService;

        public AlertSettingController(
            IAlertSettingFacadeService alertSettingFacadeService
            )
        {
            _alertSettingFacadeService = alertSettingFacadeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlertSettingInfo>>> GetAll()
        {
            var products = await _alertSettingFacadeService.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlertSettingInfo>> GetById(long id)
        {
            var product = await _alertSettingFacadeService.GetById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<AlertSettingInfo>> Create([FromBody] AlertSettingInfo info)
        {
            var created = await _alertSettingFacadeService.Create(info);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AlertSettingInfo>> Update(long id, [FromBody] AlertSettingInfo info)
        {
            var updated = await _alertSettingFacadeService.Update(id, info);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _alertSettingFacadeService.Delete(id);
            return NoContent();
        }
    }
}
