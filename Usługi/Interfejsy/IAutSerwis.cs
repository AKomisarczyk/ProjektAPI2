using Transf;
using System.Threading.Tasks;
using System.Collections.Generic;
using Modele;
using Microsoft.AspNetCore.Identity.Data;
using WymianaWaluty.Modele;
using WymianaWaluty.Controllers.Dto;

namespace WymianaWaluty.Usługi.Interfejsy
{
    public interface IAutSerwis
    {
        Task<string> LoginAsync(Transf.LoginRequest request);
        Task<bool> RegisterAsync(RegisterRqs request);
        Task<decimal> GetBalanceAsync(string username);
        Task<bool> DepositAsync(string username, decimal amount);
        Task<IEnumerable<CurrencyBalanceDto>> GetCurrencyBalancesAsync(string username);
        Task<bool> ExchangeAsync(string username, decimal amount, string currency);
       
    }
}