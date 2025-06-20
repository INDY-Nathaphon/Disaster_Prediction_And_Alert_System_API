using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Region.Interface;
using Disaster_Prediction_And_Alert_System_API.Common.Models.Base;
using Disaster_Prediction_And_Alert_System_API.Common.Models.Region;
using Disaster_Prediction_And_Alert_System_API.Common.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Disaster_Prediction_And_Alert_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionFacadeService _regionFacadeService;

        public RegionController(IRegionFacadeService regionFacadeService)
        {
            _regionFacadeService = regionFacadeService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<RegionInfo>>>> GetEntities([FromQuery] BaseFilter filter)
        {
            var result = await _regionFacadeService.GetEntities(filter);
            return Ok(ApiResponseFactory.Success(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<RegionInfo>>> GetEntityById(long id)
        {
            var result = await _regionFacadeService.GetEntityById(id);
            return Ok(ApiResponseFactory.Success(result));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<RegionInfo>>> Create([FromBody] RegionInfo info)
        {
            var result = await _regionFacadeService.Create(info);
            return Ok(ApiResponseFactory.Success(result));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<RegionInfo>>> Update(long id, [FromBody] RegionInfo info)
        {
            var result = await _regionFacadeService.Update(id, info);
            return Ok(ApiResponseFactory.Success(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _regionFacadeService.Delete(id);
            return NoContent();
        }
    }
}
