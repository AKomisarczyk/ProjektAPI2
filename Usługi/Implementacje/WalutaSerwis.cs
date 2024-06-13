using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Modele;
using Usługi.Interfejsy;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;


namespace WymianaWaluty.Usługi.Implementacje
{
    public class WalutaSerwis : IWalutaSerwis
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WalutaSerwis> _logger;

        public WalutaSerwis(HttpClient httpClient, ILogger<WalutaSerwis> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Kurs>> GetExchangeRatesAsync()
        {
            _logger.LogInformation("Fetching exchange rates from NBP API");

            var response = await _httpClient.GetAsync("https://api.nbp.pl/api/exchangerates/tables/A?format=json");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var rateTables = JsonConvert.DeserializeObject<List<RateTable>>(content);
                var rates = rateTables.SelectMany(rt => rt.Rates).ToList();
                _logger.LogInformation("Exchange rates fetched successfully from NBP API");
                return rates;
            }
            else
            {
                _logger.LogError("Error fetching exchange rates from NBP API: {StatusCode}", response.StatusCode);
                throw new HttpRequestException($"Error fetching exchange rates: {response.StatusCode}");
            }
        }

        public async Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            _logger.LogInformation("Converting currency from {FromCurrency} to {ToCurrency} with amount {Amount}", fromCurrency, toCurrency, amount);

            var rates = await GetExchangeRatesAsync();
            var fromRate = rates.FirstOrDefault(r => r.Code == fromCurrency)?.Mid ?? 0;
            var toRate = rates.FirstOrDefault(r => r.Code == toCurrency)?.Mid ?? 0;

            if (fromRate == 0 || toRate == 0)
            {
                _logger.LogError("Invalid currency codes: {FromCurrency}, {ToCurrency}", fromCurrency, toCurrency);
                throw new ArgumentException("Invalid currency codes");
            }

            var result = amount * toRate / fromRate;
            _logger.LogInformation("Currency converted successfully: {Result}", result);
            return result;
        }
    }
}