using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalonTrackApi.Contracts;
using SalonTrackApi.Entities;

namespace SalonTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController(IServiceManager service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? userId, [FromQuery] DateTime? startDate,
                                            [FromQuery] DateTime? endDate, [FromQuery] bool showDeactivated = false,
                                            [FromQuery] int page = 1)
        {
            var result = await service.IncomeService.GetFilteredIncomesAsync(userId, startDate, endDate, showDeactivated, page);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var income = await service.IncomeService.GetByIdAsync(id);
            return income is null ? NotFound() : Ok(income);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Income updated)
        {
            try
            {
                var income = await service.IncomeService.UpdateIncomeAsync(id, updated);
                return Ok(income);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await service.IncomeService.DeleteIncomeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }


        }
    }
}
