using System.Threading.Tasks;
using Transf;

namespace WymianaWaluty.Usługi.Interfejsy
{
    public interface IPayUService
    {
        Task<PayUResponse> ProcessPayment(PaymentRequest request);
    }
}