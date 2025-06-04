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

        public UserController(
            IUserFacadeService userFacadeService
            )
        {
            _userFacadeService = userFacadeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlertSettingInfo>>> GetAll()
        {
            var products = await _userFacadeService.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlertSettingInfo>> GetById(long id)
        {
            var product = await _userFacadeService.GetById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<AlertSettingInfo>> Create([FromBody] UserInfo info)
        {
            var created = await _userFacadeService.Create(info);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AlertSettingInfo>> Update(long id, [FromBody] UserInfo info)
        {
            var updated = await _userFacadeService.Update(id, info);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _userFacadeService.Delete(id);
            return NoContent();
        }

        [HttpGet("by-mobile")]
        public async Task<ActionResult<UserInfo>> GetUserByMobileNo([FromQuery] string mobileNo)
        {
            var user = await _userFacadeService.GetUserByMobileNo(mobileNo);
            return Ok(user);
        }
    }
}
