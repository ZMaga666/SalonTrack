using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalonTrackApi.Entities;
using SalonTrackApi.Repositories;

namespace SalonTrackApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IRepositoryManager _repository;

        public ExpenseController(IRepositoryManager repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await _repository.Expense.GetAllAsync();
            return Ok(expenses);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Expense expense)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            expense.Date = DateTime.Now;
            await _repository.Expense.CreateAsync(expense);
            await _repository.SaveAsync();
            return Ok(expense);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _repository.Expense.GetByIdAsync(id);
            if (expense is null) return NotFound();

            _repository.Expense.Delete(expense);
            await _repository.SaveAsync();
            return NoContent();
        }
    }

}
