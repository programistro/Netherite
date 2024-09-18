using Netherite.Domain;

namespace Netherite.Interface;

public interface IFavoritesRepository
{
    Task<List<CurrencyPairs>> Get(Guid userId);

    Task<Guid> Create(Guid userId, Guid currencyPairsId);

    Task<bool> Delete(Guid userId, Guid currencyPairsId);
}