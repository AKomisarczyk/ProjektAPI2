using WymianaWaluty.Usługi.Interfejsy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modele;
using Transf;

namespace WymianaWaluty.Usługi.Implementacje
{
    public class TransakcjaSerwis : ITransakcjaSerwis
    {
        private readonly List<Transakcja> _transactions = new List<Transakcja>();

        public Task<IEnumerable<Transakcja>> GetTransactionsAsync()
        {
            return Task.FromResult<IEnumerable<Transakcja>>(_transactions);
        }

        public Task<Transakcja> GetTransactionByIdAsync(int id)
        {
            var transaction = _transactions.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(transaction);
        }

        public Task<Transakcja> CreateTransactionAsync(TransakcjaR request)
        {
            var transaction = new Transakcja
            {
                Id = _transactions.Count + 1,
                FromCurrency = request.FromCurrency,
                ToCurrency = request.ToCurrency,
                Amount = request.Amount,
                ConvertedAmount = request.Amount * 4.2m, // Example conversion rate
                Date = DateTime.Now
            };
            _transactions.Add(transaction);
            return Task.FromResult(transaction);
        }

        public Task<Transakcja> UpdateTransactionAsync(int id, TransakcjaR request)
        {
            var transaction = _transactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                transaction.FromCurrency = request.FromCurrency;
                transaction.ToCurrency = request.ToCurrency;
                transaction.Amount = request.Amount;
                transaction.ConvertedAmount = request.Amount * 4.2m; // Example conversion rate
            }
            return Task.FromResult(transaction);
        }

        public Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = _transactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                _transactions.Remove(transaction);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}