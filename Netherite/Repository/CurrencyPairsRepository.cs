using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Data;
using Netherite.Domain;
using Netherite.Domain.Entitys;
using Netherite.Interface;

namespace Netherite.Repository;

 public class CurrencyPairsRepository : ICurrencyPairsRepository
  {
    private readonly NetheriteDbContext _context;

    public CurrencyPairsRepository(NetheriteDbContext context) => this._context = context;

    public async Task<List<CurrencyPairs>> GetCurrencyPairs()
    {
      List<CurrencyPairsEntity> currencyPairsEntities = await this._context.CurrencyPairs.AsNoTracking<CurrencyPairsEntity>().ToListAsync<CurrencyPairsEntity>();
      List<CurrencyPairs> currencyPairs = currencyPairsEntities.Select<CurrencyPairsEntity, CurrencyPairs>((Func<CurrencyPairsEntity, CurrencyPairs>) (cp => CurrencyPairs.Create(cp.Id, cp.Name, cp.NameTwo, cp.Icon, cp.InterestRate))).ToList<CurrencyPairs>();
      List<CurrencyPairs> currencyPairs1 = currencyPairs;
      currencyPairsEntities = (List<CurrencyPairsEntity>) null;
      currencyPairs = (List<CurrencyPairs>) null;
      return currencyPairs1;
    }

    public async Task<Guid> Create(CurrencyPairs currencyPairs)
    {
      CurrencyPairsEntity currencyPairsEntity = new CurrencyPairsEntity()
      {
        Id = currencyPairs.Id,
        Name = currencyPairs.Name,
        NameTwo = currencyPairs.NameTwo,
        Icon = currencyPairs.Icon,
        InterestRate = currencyPairs.InterestRate
      };
      EntityEntry<CurrencyPairsEntity> entityEntry = await this._context.CurrencyPairs.AddAsync(currencyPairsEntity);
      int num = await this._context.SaveChangesAsync();
      Guid id = currencyPairsEntity.Id;
      currencyPairsEntity = (CurrencyPairsEntity) null;
      return id;
    }

    public async Task<bool> Delete(Guid currencyPairsId)
    {
      CurrencyPairsEntity currencyPairsEntity = await this._context.CurrencyPairs.FindAsync((object) currencyPairsId);
      if (currencyPairsEntity == null)
        return false;
      this._context.CurrencyPairs.Remove(currencyPairsEntity);
      int num = await this._context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Update(Guid currencyPairsId, CurrencyPairs currencyPairs)
    {
      CurrencyPairsEntity currencyPairsEntity = await this._context.CurrencyPairs.FindAsync((object) currencyPairsId);
      if (currencyPairsEntity == null)
        return false;
      if (!string.IsNullOrEmpty(currencyPairs.Name))
        currencyPairsEntity.Name = currencyPairs.Name;
      if (!string.IsNullOrEmpty(currencyPairs.NameTwo))
        currencyPairsEntity.NameTwo = currencyPairs.NameTwo;
      if (!string.IsNullOrEmpty(currencyPairs.Icon))
        currencyPairsEntity.Icon = currencyPairs.Icon;
      currencyPairsEntity.InterestRate = currencyPairs.InterestRate;
      this._context.CurrencyPairs.Update(currencyPairsEntity);
      int num = await this._context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> UploadIcon(Guid currencyPairId, string fileUrl)
    {
      CurrencyPairsEntity currencyPairsEntity = await this._context.CurrencyPairs.FindAsync((object) currencyPairId);
      if (currencyPairsEntity == null)
        return false;
      currencyPairsEntity.Icon = fileUrl;
      this._context.CurrencyPairs.Update(currencyPairsEntity);
      int num = await this._context.SaveChangesAsync();
      return true;
    }
  }