using Microsoft.AspNetCore.Mvc;
using Transf;
using Usługi.Interfejsy;
using System.Linq;
using System.Threading.Tasks;
using WymianaWaluty.Modele;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using WymianaWaluty.Usługi.Implementacje;
using WymianaWaluty.Usługi.Interfejsy;
using Microsoft.AspNetCore.Identity.Data;
using WymianaWaluty.Usługi.Implementacje.WymianaWaluty.Usługi.Implementacje;

namespace WymianaWaluty.Kontrolery
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutentKontroler : ControllerBase
    {
        private readonly IAutSerwis _autSerwis;

        public AutentKontroler(IAutSerwis autSerwis)
        {
            _autSerwis = autSerwis;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRqs request)
        {
            var result = await _autSerwis.RegisterAsync(request);
            if (result)
            {
                return Ok(new { message = "Registration successful" });
            }

            return BadRequest(new { message = "Registration failed" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Transf.LoginRequest request)
        {
            var token = await _autSerwis.LoginAsync(request);
            if (token != null)
            {
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Login failed" });
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance([FromQuery] string username)
        {
            var balance = await _autSerwis.GetBalanceAsync(username);
            return Ok(new { balance });
        }

        [HttpGet("currency-balances")]
        public async Task<IActionResult> GetCurrencyBalances([FromQuery] string username)
        {
            var balances = await _autSerwis.GetCurrencyBalancesAsync(username);
            return Ok( balances );
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(DepositRequest request)
        {
            var result = await _autSerwis.DepositAsync(request.Username, request.Amount);
            if (result)
            {
                return Ok(new { message = "Deposit successful" });
            }

            return BadRequest(new { message = "Deposit failed" });
        }

        [HttpPost("exchange")]
        public async Task<IActionResult> Exchange(ExchangeRequest request)
        {
            var result = await _autSerwis.ExchangeAsync(request.Username, request.Amount, request.Currency);
            if (result)
            {
                return Ok(new { message = "Exchange successful" });
            }

            return BadRequest(new { message = "Exchange failed" });
        }
    }
}