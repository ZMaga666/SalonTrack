using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalonTrackApi.Contracts;
using SalonTrackApi.DTO;

namespace SalonTrackApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IServiceManager service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string status = "active")
        {
            var users = await service.UserService.GetFilteredUsersAsync(startDate, endDate, status);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await service.UserService.GetUserByIdAsync(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            try
            {
                var user = await service.UserService.CreateUserAsync(dto);
                return Ok(new { user.Id, user.UserName });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserEditDto dto)
        {
            try
            {
                await service.UserService.UpdateUserAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> Activate(string id)
        {
            try
            {
                await service.UserService.ActivateUserAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await service.UserService.SoftDeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}
