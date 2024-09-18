using Netherite.Domain;

namespace Netherite.Interface;

public interface IIntervalsRepository
{
    Task<List<Interval>> GetByPairsId(Guid currencyPairsId);

    Task<Guid> Create(Interval interval, Guid pairsId);

    Task<bool> Delete(Guid intervalId);

    Task<bool> Update(Guid intervalId, Interval interval);
}