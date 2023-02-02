namespace GameteoDTO.DTO
{
    public class CurrencyRate
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Day { get; set; } = 1;
        public int Year { get; set; } = 1;
        public float Value { get; set; } = 0;
        public string CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
    }
}