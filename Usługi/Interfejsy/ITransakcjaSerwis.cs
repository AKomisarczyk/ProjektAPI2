using System.Collections.Generic;
using System.Threading.Tasks;
using Modele;
using Transf;

namespace WymianaWaluty.Usługi.Interfejsy
{
    public interface ITransakcjaSerwis
    {
        Task<IEnumerable<Transakcja>> GetTransactionsAsync();
        Task<Transakcja> GetTransactionByIdAsync(int id);
        Task<Transakcja> CreateTransactionAsync(TransakcjaR request);
        Task<Transakcja> UpdateTransactionAsync(int id, TransakcjaR request);
        Task<bool> DeleteTransactionAsync(int id);
    }
}