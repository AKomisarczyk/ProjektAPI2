using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WymianaWaluty.Usługi.Interfejsy;
using Microsoft.Extensions.Logging;
using Usługi.Interfejsy;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WymianaWaluty.Usługi.Interfejsy;
using Microsoft.Extensions.Logging;

namespace WymianaWaluty.Kontrolery
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalutKontroler : ControllerBase
    {
        private readonly IWalutaSerwis _walutaSerwis;
        private readonly ILogger<WalutKontroler> _logger;

        public WalutKontroler(IWalutaSerwis walutaSerwis, ILogger<WalutKontroler> logger)
        {
            _walutaSerwis = walutaSerwis;
            _logger = logger;
        }

        [HttpGet("exchange-rates")]
        public async Task<IActionResult> GetExchangeRates()
        {
            _logger.LogInformation("GetExchangeRates called");

            try
            {
                var rates = await _walutaSerwis.GetExchangeRatesAsync();
                _logger.LogInformation("Exchange rates fetched successfully");
                return Ok(rates);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching exchange rates");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertCurrency([FromBody] ConversionRequest request)
        {
            _logger.LogInformation("ConvertCurrency called with {FromCurrency}, {ToCurrency}, {Amount}", request.FromCurrency, request.ToCurrency, request.Amount);

            try
            {
                var result = await _walutaSerwis.ConvertCurrencyAsync(request.FromCurrency, request.ToCurrency, request.Amount);
                _logger.LogInformation("Currency converted successfully");
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error converting currency");
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class ConversionRequest
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
    }
}