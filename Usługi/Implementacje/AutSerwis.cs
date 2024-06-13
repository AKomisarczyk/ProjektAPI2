using System.Linq;
using System.Threading.Tasks;
using WymianaWaluty.Modele;
using WymianaWaluty.Usługi.Interfejsy;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Modele;
using Transf;
using Usługi.Interfejsy;
using Microsoft.AspNetCore.Identity.Data;
using WymianaWaluty.Controllers.Dto;

namespace WymianaWaluty.Usługi.Implementacje
{
    namespace WymianaWaluty.Usługi.Implementacje
    {
        public class AutSerwis : IAutSerwis
        {
            private readonly ApplicationDbContext _context;
            private readonly ILogger<AutSerwis> _logger;
            private readonly IWalutaSerwis _walutaSerwis;

            public AutSerwis(ApplicationDbContext context, ILogger<AutSerwis> logger, IWalutaSerwis walutaSerwis)
            {
                _context = context;
                _logger = logger;
                _walutaSerwis = walutaSerwis;
            }

            public async Task<string> LoginAsync(Transf.LoginRequest request)
            {
                _logger.LogInformation($"Attempting to login user: {request.Username}");

                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);
                if (user == null)
                {
                    _logger.LogWarning($"Login failed for user: {request.Username} - User not found");
                    return null;
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
                if (!isPasswordValid)
                {
                    _logger.LogWarning($"Login failed for user: {request.Username} - Invalid password");
                    return null;
                }

                var token = "generated-jwt-token";
                _logger.LogInformation($"Token generated for user: {request.Username}");

                return token;
            }

            public async Task<bool> RegisterAsync(RegisterRqs request)
            {
                _logger.LogInformation($"Registering user: {request.Username}");

                var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning($"User already exists: {request.Username}");
                    return false;
                }

                var user = new User
                {
                    Username = request.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Balance = 0m,
                    CurrencyBalances = new List<CurrencyBalance>
                {
                    new CurrencyBalance { Currency = "USD", Amount = 0m },
                    new CurrencyBalance { Currency = "EUR", Amount = 0m },
                    new CurrencyBalance { Currency = "GBP", Amount = 0m },
                    new CurrencyBalance { Currency = "CHF", Amount = 0m },
                    new CurrencyBalance { Currency = "JPY", Amount = 0m },
                    new CurrencyBalance { Currency = "CAD", Amount = 0m }
                }
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User registered successfully: {request.Username}");
                return true;
            }

            public async Task<decimal> GetBalanceAsync(string username)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                return user?.Balance ?? 0;
            }

            public async Task<bool> DepositAsync(string username, decimal amount)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    return false;
                }

                user.Balance += amount;
                await _context.SaveChangesAsync();
                return true;
            }

            public async Task<bool> ExchangeAsync(string username, decimal amount, string currency)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                if (user == null || user.Balance < amount)
                {
                    return false;
                }

                var rates = await _walutaSerwis.GetExchangeRatesAsync();
                var rate = rates.SingleOrDefault(r => r.Code == currency)?.Mid ?? 0;
                if (rate == 0)
                {
                    return false;
                }

                var exchangedAmount = amount / rate;
                user.Balance -= amount;

                var currencyBalance = await _context.CurrencyBalances
                    .SingleOrDefaultAsync(cb => cb.UserId == user.Id && cb.Currency == currency);

                if (currencyBalance == null)
                {
                    currencyBalance = new CurrencyBalance
                    {
                        UserId = user.Id,
                        Currency = currency,
                        Amount = exchangedAmount
                    };
                    _context.CurrencyBalances.Add(currencyBalance);
                }
                else
                {
                    currencyBalance.Amount += exchangedAmount;
                    _context.CurrencyBalances.Update(currencyBalance);
                }

                await _context.SaveChangesAsync();
                return true;
            }

            public async Task<IEnumerable<CurrencyBalanceDto>> GetCurrencyBalancesAsync(string username)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    return Enumerable.Empty<CurrencyBalanceDto>();
                }

                var balances = await _context.CurrencyBalances
                    .Where(cb => cb.UserId == user.Id)
                    .Select(cb => new CurrencyBalanceDto
                    {
                        Currency = cb.Currency,
                        Amount = cb.Amount
                    })
                    .ToListAsync();

                return balances;
            }
        }
    }
}