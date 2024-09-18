using Netherite.Domain;
using Netherite.Interface;

namespace Netherite.Service;

public class FavoritesService : IFavoritesService
{
    private readonly IFavoritesRepository _favoritesRepository;

    public FavoritesService(IFavoritesRepository favoritesRepository)
    {
        this._favoritesRepository = favoritesRepository;
    }

    public Task<Guid> CreateFavorites(Guid userId, Guid currencyPairsId)
    {
        return this._favoritesRepository.Create(userId, currencyPairsId);
    }

    public Task<bool> DeleteFavorites(Guid userId, Guid currencyPairsId)
    {
        return this._favoritesRepository.Delete(userId, currencyPairsId);
    }

    public Task<List<CurrencyPairs>> GetFavorites(Guid userId)
    {
        return this._favoritesRepository.Get(userId);
    }
}