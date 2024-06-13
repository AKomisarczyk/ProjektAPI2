using Microsoft.AspNetCore.Mvc;
using WymianaWaluty.Usługi.Interfejsy;
using System.Threading.Tasks;
using Transf;
using WymianaWaluty.Usługi.Implementacje;

namespace WymianaWaluty.Kontrolery
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransakcjeKontroler : ControllerBase
    {
        private readonly ITransakcjaSerwis _transactionService;
        private readonly IPayUService _payUService;

        public TransakcjeKontroler(ITransakcjaSerwis transactionService, IPayUService payUService)
        {
            _transactionService = transactionService;
            _payUService = payUService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _transactionService.GetTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransakcjaR request)
        {
            var transaction = await _transactionService.CreateTransactionAsync(request);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
        }

        [HttpPost("process-payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            var response = await _payUService.ProcessPayment(request);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransakcjaR request)
        {
            var updatedTransaction = await _transactionService.UpdateTransactionAsync(id, request);
            if (updatedTransaction == null)
            {
                return NotFound();
            }
            return Ok(updatedTransaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var result = await _transactionService.DeleteTransactionAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}