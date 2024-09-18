using Netherite.Domain;

namespace Netherite.Interface;

public class IntervalServices : IIntervalServices
{
    private readonly IIntervalsRepository _intervalRepository;

    public IntervalServices(IIntervalsRepository intervalRepository)
    {
        this._intervalRepository = intervalRepository;
    }

    public Task<Guid> CreateInterval(Interval interval, Guid pairsId)
    {
        return this._intervalRepository.Create(interval, pairsId);
    }

    public Task<bool> DeleteInterval(Guid intervalId)
    {
        return this._intervalRepository.Delete(intervalId);
    }

    public Task<List<Interval>> GetIntervalByPairsId(Guid pairsId)
    {
        return this._intervalRepository.GetByPairsId(pairsId);
    }

    public Task<bool> UpdateInterval(Guid intervalId, Interval interval)
    {
        return this._intervalRepository.Update(intervalId, interval);
    }
}