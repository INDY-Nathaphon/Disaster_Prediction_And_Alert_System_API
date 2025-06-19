using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Region.Interface;
using Disaster_Prediction_And_Alert_System_API.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Disaster_Prediction_And_Alert_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionFacadeService _regionFacadeService;
        private readonly ILogger<RegionController> _logger;

        public RegionController(ILogger<RegionController> logger, IRegionFacadeService regionFacadeService)
        {
            _regionFacadeService = regionFacadeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlertSettingInfo>>> GetAll()
        {
            try
            {
                var result = await _regionFacadeService.GetAll();
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
                var result = await _regionFacadeService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<AlertSettingInfo>> Create([FromBody] RegionInfo info)
        {
            try
            {
                var created = await _regionFacadeService.Create(info);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AlertSettingInfo>> Update(long id, [FromBody] RegionInfo info)
        {
            try
            {
                var updated = await _regionFacadeService.Update(id, info);
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
                await _regionFacadeService.Delete(id);
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
