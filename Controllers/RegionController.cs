using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Region.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Disaster_Prediction_And_Alert_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        IRegionFacadeService _regionFacadeService;

        public RegionController(IRegionFacadeService regionFacadeService)
        {
            _regionFacadeService = regionFacadeService;
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
                return BadRequest(ex);
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
                return BadRequest(ex);
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
                return BadRequest(ex);
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
                return BadRequest(ex);
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
                return BadRequest(ex);
            }
        }
    }
}
