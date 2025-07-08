using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalonTrackApi.Contracts;
using SalonTrackApi.Entities;

namespace SalonTrackApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController(IServiceManager service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await service.ServiceService.GetAllServiceAsync();
            return Ok(services);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Service serviceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await service.ServiceService.CreateServiceAsync(serviceDto);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await service.ServiceService.DeleteServiceAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}
