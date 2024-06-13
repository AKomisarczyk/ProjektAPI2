using Modele;

namespace WymianaWaluty.Modele
{
    public class CurrencyBalance
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }

        public User User { get; set; }
    }
}