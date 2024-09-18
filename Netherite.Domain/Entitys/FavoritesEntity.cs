namespace Netherite.Domain.Entitys;

public class FavoritesEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid CurrencyPairsId { get; set; }

    public UserEntity? User { get; set; } = (UserEntity) null;

    public CurrencyPairsEntity? CurrencyPairs { get; set; } = (CurrencyPairsEntity) null;
}