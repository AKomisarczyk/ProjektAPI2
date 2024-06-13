using WymianaWaluty.Modele;

namespace Modele
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public ICollection<CurrencyBalance> CurrencyBalances { get; set; }
    }
}