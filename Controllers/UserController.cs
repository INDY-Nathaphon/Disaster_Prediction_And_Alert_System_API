using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.User.Interface;
using Disaster_Prediction_And_Alert_System_API.Common.Models.Base;
using Disaster_Prediction_And_Alert_System_API.Common.Models.Response;
using Disaster_Prediction_And_Alert_System_API.Common.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Disaster_Prediction_And_Alert_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserFacadeService _userFacadeService;

        public UserController(IUserFacadeService userFacadeService)
        {
            _userFacadeService = userFacadeService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<UserInfo>>>> GetEntities([FromQuery] BaseFilter filter)
        {
            var result = await _userFacadeService.GetEntities(filter);
            return Ok(ApiResponseFactory.Success(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserInfo>>> GetEntityById(long id)
        {
            var result = await _userFacadeService.GetEntityById(id);
            return Ok(ApiResponseFactory.Success(result));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserInfo>>> Create([FromBody] UserInfo info)
        {
            var result = await _userFacadeService.Create(info);
            return Ok(ApiResponseFactory.Success(result));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserInfo>>> Update(long id, [FromBody] UserInfo info)
        {
            var result = await _userFacadeService.Update(id, info);
            return Ok(ApiResponseFactory.Success(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _userFacadeService.Delete(id);
            return NoContent();
        }

        [HttpGet("by-mobile")]
        public async Task<ActionResult<ApiResponse<UserInfo>>> GetUserByMobileNo([FromQuery] string mobileNo)
        {
            var result = await _userFacadeService.GetUserByMobileNo(mobileNo);
            return Ok(ApiResponseFactory.Success(result));
        }
    }
}
