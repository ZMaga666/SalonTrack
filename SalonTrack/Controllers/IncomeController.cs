using Microsoft.AspNetCore.Mvc;
using SalonTrack.Models;
using SalonTrack.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SalonTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly SalonContext _context;

        public IncomeController(SalonContext context)
        {
            _context = context;
        }

        // GET: api/income
        [HttpGet]
        public IActionResult GetAll()
        {
            var incomes = _context.Incomes.OrderByDescending(i => i.Date).ToList();
            return Ok(incomes);
        }

        // GET: api/income/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var income = _context.Incomes.FirstOrDefault(i => i.Id == id);
            if (income == null) return NotFound();

            return Ok(income);
        }

        // POST: api/income
        [HttpPost]
        public IActionResult Create([FromBody] Income income)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            income.Date = income.Date.ToLocalTime(); // Optional
            _context.Incomes.Add(income);
            _context.SaveChanges();
            return Ok(income);
        }

        // PUT: api/income/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Income updatedIncome)
        {
            var income = _context.Incomes.FirstOrDefault(i => i.Id == id);
            if (income == null) return NotFound();

            income.Date = updatedIncome.Date;
            income.Amount = updatedIncome.Amount;

            _context.SaveChanges();
            return Ok(income);
        }

        // DELETE: api/income/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var income = _context.Incomes.FirstOrDefault(i => i.Id == id);
            if (income == null) return NotFound();

            _context.Incomes.Remove(income);
            _context.SaveChanges();
            return NoContent();
        }

        // GET: api/income/filter?startDate=2025-05-01&endDate=2025-05-06
        [HttpGet("filter")]
        public IActionResult FilterByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var filtered = _context.Incomes
                .Where(i => i.Date.Date >= startDate.Date && i.Date.Date <= endDate.Date)
                .OrderByDescending(i => i.Date)
                .ToList();

            return Ok(filtered);
        }
    }
}
