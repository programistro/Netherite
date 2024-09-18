using Netherite.Domain;

namespace Netherite.Interface;

public interface IFavoritesService
{
    Task<List<CurrencyPairs>> GetFavorites(Guid userId);

    Task<Guid> CreateFavorites(Guid userId, Guid currencyPairsId);

    Task<bool> DeleteFavorites(Guid userId, Guid currencyPairsId);
}