namespace Netherite.Domain.Entitys;

public class CurrencyPairsEntity
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? NameTwo { get; set; }

    public string? Icon { get; set; }

    public Decimal InterestRate { get; set; }

    public ICollection<CurrencyPairsIntervalEntity> CurrencyPairsIntervals { get; set; } = (ICollection<CurrencyPairsIntervalEntity>) new List<CurrencyPairsIntervalEntity>();

    public ICollection<FavoritesEntity> Favorites { get; set; } = (ICollection<FavoritesEntity>) new List<FavoritesEntity>();
}