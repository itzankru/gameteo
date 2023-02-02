namespace GameteoDTO.DTO
{
    public class Currency
    {
        public string Id { get; set; }
        public int LotSize { get; set; } = 1;

        public virtual List<CurrencyRate> CurrencyRates { get; set; }
    }
}
