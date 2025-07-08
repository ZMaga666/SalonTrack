using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalonTrackApi.Contracts;
using SalonTrackApi.Entities;

namespace SalonTrackApi.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class ServiceTaskController(IServiceManager service, UserManager<User> userManager) : ControllerBase
        {
            [HttpGet("today")]
            public async Task<IActionResult> GetTodayTasks()
            {
                var tasks = await service.ServiceTaskService.GetTodayTasksAsync();
                return Ok(tasks);
            }

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] ServiceTask task)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = User.IsInRole("Admin") && !string.IsNullOrEmpty(task.UserId)
                             ? task.UserId
                             : userManager.GetUserId(User); 

                try
                {
                    var created = await service.ServiceTaskService.CreateServiceTaskAsync(task, userId);
                    return Ok(created);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                try
                {
                    await service.ServiceTaskService.DeleteServiceTaskAsync(id);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }
        }
}

