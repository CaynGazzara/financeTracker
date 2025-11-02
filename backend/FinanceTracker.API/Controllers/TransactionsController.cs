// Controllers/TransactionsController.cs
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.API.Models;
using FinanceTracker.API.Services;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IFinanceService _financeService;

        public TransactionsController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var transactions = _financeService.GetAllTransactions();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var transaction = _financeService.GetTransactionById(id);
            if (transaction == null) return NotFound();
            return Ok(transaction);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Transaction transaction)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = _financeService.CreateTransaction(transaction);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Transaction transaction)
        {
            Console.WriteLine($"=== UPDATE TRANSACTION {id} ===");
            Console.WriteLine($"Description: {transaction.Description}");
            Console.WriteLine($"Amount: {transaction.Amount}");
            Console.WriteLine($"Type: {transaction.Type}");
            Console.WriteLine($"CategoryId: {transaction.CategoryId}");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = _financeService.UpdateTransaction(id, transaction);
            if (updated == null) return NotFound();

            Console.WriteLine($"=== UPDATE SUCCESS ===");
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _financeService.DeleteTransaction(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}