namespace Netherite.Domain.Entitys;

public class IntervalEntity
{
    public Guid Id { get; set; }

    public int Time { get; set; }

    public ICollection<CurrencyPairsIntervalEntity> CurrencyPairsIntervals { get; set; } = (ICollection<CurrencyPairsIntervalEntity>) new List<CurrencyPairsIntervalEntity>();
}