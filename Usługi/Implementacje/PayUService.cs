using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Transf;
using WymianaWaluty.Usługi.Interfejsy;

namespace WymianaWaluty.Usługi.Implementacje
{
    public class PayUService : IPayUService
    {
        private readonly HttpClient _httpClient;

        public PayUService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PayUResponse> ProcessPayment(PaymentRequest request)
        {
            var requestJson = JsonSerializer.Serialize(request);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            // Set up the headers and authentication as per PayU API documentation
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your_access_token");

            var response = await _httpClient.PostAsync("https://secure.snd.payu.com/api/v2_1/orders", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PayUResponse>(responseJson);
        }
    }
}