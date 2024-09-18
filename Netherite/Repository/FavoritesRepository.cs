using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Data;
using Netherite.Domain;
using Netherite.Domain.Entitys;
using Netherite.Interface;

namespace Netherite.Repository;

public class FavoritesRepository : IFavoritesRepository
  {
    private readonly NetheriteDbContext _context;

    public FavoritesRepository(NetheriteDbContext context) => this._context = context;

    public async Task<List<CurrencyPairs>> Get(Guid userId)
    {
      List<FavoritesEntity> favoritesEntity = await this._context.Favorites.Where<FavoritesEntity>((Expression<Func<FavoritesEntity, bool>>) (f => f.UserId == userId)).Include<FavoritesEntity, CurrencyPairsEntity>((Expression<Func<FavoritesEntity, CurrencyPairsEntity>>) (f => f.CurrencyPairs)).ToListAsync<FavoritesEntity>();
      List<CurrencyPairs> currencyPairs = favoritesEntity.Select<FavoritesEntity, CurrencyPairs>((Func<FavoritesEntity, CurrencyPairs>) (f => CurrencyPairs.Create(f.CurrencyPairs.Id, f.CurrencyPairs.Name, f.CurrencyPairs.NameTwo, f.CurrencyPairs.Icon, f.CurrencyPairs.InterestRate))).ToList<CurrencyPairs>();
      List<CurrencyPairs> currencyPairsList = currencyPairs;
      favoritesEntity = (List<FavoritesEntity>) null;
      currencyPairs = (List<CurrencyPairs>) null;
      return currencyPairsList;
    }

    public async Task<Guid> Create(Guid userId, Guid currencyPairsId)
    {
      CurrencyPairsEntity currencyPairsEntity = await this._context.CurrencyPairs.FirstOrDefaultAsync<CurrencyPairsEntity>((Expression<Func<CurrencyPairsEntity, bool>>) (cp => cp.Id == currencyPairsId));
      UserEntity userEntity = await this._context.Users.FirstOrDefaultAsync<UserEntity>((Expression<Func<UserEntity, bool>>) (u => u.Id == userId));
      if (currencyPairsEntity == null || userEntity == null)
        return Guid.Empty;
      FavoritesEntity favoritesEntity = new FavoritesEntity()
      {
        Id = Guid.NewGuid(),
        UserId = userId,
        CurrencyPairsId = currencyPairsId,
        User = userEntity,
        CurrencyPairs = currencyPairsEntity
      };
      EntityEntry<FavoritesEntity> entityEntry = await this._context.Favorites.AddAsync(favoritesEntity);
      int num = await this._context.SaveChangesAsync();
      return favoritesEntity.Id;
    }

    public async Task<bool> Delete(Guid userId, Guid currencyPairsId)
    {
      FavoritesEntity favoritesEntity = await this._context.Favorites.FirstOrDefaultAsync<FavoritesEntity>((Expression<Func<FavoritesEntity, bool>>) (f => f.UserId == userId && f.CurrencyPairsId == currencyPairsId));
      if (favoritesEntity == null)
        return false;
      this._context.Favorites.Remove(favoritesEntity);
      int num = await this._context.SaveChangesAsync();
      return true;
    }
  }