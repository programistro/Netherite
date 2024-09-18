namespace Netherite.Domain.Entitys;

public class CurrencyPairsIntervalEntity
{
    public Guid Id { get; set; }

    public Guid CurrencyPairsId { get; set; }

    public CurrencyPairsEntity? CurrencyPairs { get; set; } = (CurrencyPairsEntity) null;

    public Guid IntervalId { get; set; }

    public IntervalEntity? Interval { get; set; } = (IntervalEntity) null;
}