using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Data;
using Netherite.Domain;
using Netherite.Domain.Entitys;
using Netherite.Interface;

namespace Netherite.Repository;

 public class IntervalsRepository : IIntervalsRepository
  {
    private readonly NetheriteDbContext _context;

    public IntervalsRepository(NetheriteDbContext context) => this._context = context;

    public async Task<List<Interval>> GetByPairsId(Guid currencyPairsId)
    {
      List<CurrencyPairsIntervalEntity> currencyPairsIntervalsEntities = await this._context.CurrencyPairsIntervals.AsNoTracking<CurrencyPairsIntervalEntity>().Where<CurrencyPairsIntervalEntity>((Expression<Func<CurrencyPairsIntervalEntity, bool>>) (cpi => cpi.CurrencyPairsId == currencyPairsId)).ToListAsync<CurrencyPairsIntervalEntity>();
      List<Interval> intervals = new List<Interval>();
      foreach (CurrencyPairsIntervalEntity pairsIntervalEntity in currencyPairsIntervalsEntities)
      {
        CurrencyPairsIntervalEntity entity = pairsIntervalEntity;
        IntervalEntity interval = await this._context.Intervals.AsNoTracking<IntervalEntity>().Where<IntervalEntity>((Expression<Func<IntervalEntity, bool>>) (i => i.Id == entity.IntervalId)).FirstOrDefaultAsync<IntervalEntity>();
        if (interval != null)
          intervals.Add(Interval.Create(interval.Id, interval.Time));
        interval = (IntervalEntity) null;
      }
      List<Interval> byPairsId = intervals;
      currencyPairsIntervalsEntities = (List<CurrencyPairsIntervalEntity>) null;
      intervals = (List<Interval>) null;
      return byPairsId;
    }

    public async Task<Guid> Create(Interval interval, Guid pairsId)
    {
      IntervalEntity intervalEntity = new IntervalEntity()
      {
        Id = interval.Id,
        Time = interval.Time
      };
      CurrencyPairsEntity currencyPairEntity = await this._context.CurrencyPairs.FirstOrDefaultAsync<CurrencyPairsEntity>((Expression<Func<CurrencyPairsEntity, bool>>) (cp => cp.Id == pairsId));
      if (currencyPairEntity == null)
        return Guid.Empty;
      CurrencyPairsIntervalEntity currencyPairsIntervalsEntities = new CurrencyPairsIntervalEntity()
      {
        Id = Guid.NewGuid(),
        CurrencyPairsId = currencyPairEntity.Id,
        CurrencyPairs = currencyPairEntity,
        IntervalId = intervalEntity.Id,
        Interval = intervalEntity
      };
      EntityEntry<IntervalEntity> entityEntry1 = await this._context.Intervals.AddAsync(intervalEntity);
      EntityEntry<CurrencyPairsIntervalEntity> entityEntry2 = await this._context.CurrencyPairsIntervals.AddAsync(currencyPairsIntervalsEntities);
      int num = await this._context.SaveChangesAsync();
      return intervalEntity.Id;
    }

    public async Task<bool> Delete(Guid intervalId)
    {
      IntervalEntity intervalEntity = await this._context.Intervals.FindAsync((object) intervalId);
      if (intervalEntity == null)
        return false;
      this._context.Intervals.Remove(intervalEntity);
      int num = await this._context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Update(Guid intervalId, Interval interval)
    {
      IntervalEntity intervalEntity = await this._context.Intervals.FindAsync((object) intervalId);
      if (intervalEntity == null)
        return false;
      intervalEntity.Time = interval.Time;
      this._context.Intervals.Update(intervalEntity);
      int num = await this._context.SaveChangesAsync();
      return true;
    }
  }