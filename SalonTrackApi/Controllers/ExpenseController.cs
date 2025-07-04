using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalonTrackApi.Contracts;
using SalonTrackApi.Entities;
using SalonTrackApi.Repositories;
using SalonTrackApi.Services;

namespace SalonTrackApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController(IServiceManager services) : ControllerBase
    {
        private readonly IServiceManager _services = services;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await _services.ExpenseService.GetAllExpenseAsync();

            return Ok(expenses);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Expense expense)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _services.ExpenseService.CreateExpenseAsync(expense);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _services.ExpenseService.DeleteExpenseAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var all = await _services.ExpenseService.GetAllExpenseAsync();
            var expense = all.FirstOrDefault(e => e.Id == id);

            return expense is null ? NotFound() : Ok(expense);
        }

    }
}
