using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.User.Interface;
using Disaster_Prediction_And_Alert_System_API.Model;
using Microsoft.AspNetCore.Mvc;

namespace Disaster_Prediction_And_Alert_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserFacadeService _userFacadeService;

        public UserController(IUserFacadeService userFacadeService)
        {
            _userFacadeService = userFacadeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlertSettingInfo>>> GetAll()
        {
            try
            {
                var result = await _userFacadeService.GetAll();
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
                var result = await _userFacadeService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AlertSettingInfo>> Create([FromBody] UserInfo info)
        {
            try
            {
                var created = await _userFacadeService.Create(info);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AlertSettingInfo>> Update(long id, [FromBody] UserInfo info)
        {
            try
            {
                var updated = await _userFacadeService.Update(id, info);
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
                await _userFacadeService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("by-mobile")]
        public async Task<ActionResult<UserInfo>> GetUserByMobileNo([FromQuery] string mobileNo)
        {
            try
            {
                var user = await _userFacadeService.GetUserByMobileNo(mobileNo);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
