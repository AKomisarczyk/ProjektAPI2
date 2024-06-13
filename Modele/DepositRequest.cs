namespace WymianaWaluty.Modele
{
    public class DepositRequest
    {
        public string Username { get; set; }
        public decimal Amount { get; set; }
    }

    public class ExchangeRqs
    {
        public string Username { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}