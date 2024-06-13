using Modele;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usługi.Interfejsy
{
    public interface IWalutaSerwis
    {
        Task<IEnumerable<Kurs>> GetExchangeRatesAsync();
        Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount);
    }
}