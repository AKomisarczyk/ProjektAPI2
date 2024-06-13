namespace Modele
{
    public class Kurs
    {
        public string Currency { get; set; }
        public string Code { get; set; }
        public decimal Mid { get; set; }
    }

    public class RateTable
    {
        public string Table { get; set; }
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public List<Kurs> Rates { get; set; }
    }
}
