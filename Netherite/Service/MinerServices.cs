using Netherite.Domain;
using Netherite.Interface;

namespace Netherite.Service;

public class MinerServices : IMinerServices
{
    private readonly IMinerRepository _minerRepository;

    public MinerServices(IMinerRepository minerRepository)
    {
        this._minerRepository = minerRepository;
    }

    public async Task<bool> EndMining(Guid userId)
    {
        bool flag = await this._minerRepository.End(userId);
        return flag;
    }

    public async Task<(double remainingTime, bool isFound)> GetCurrentTime(Guid userId)
    {
        (double, bool) currentTime = await this._minerRepository.Get(userId);
        return currentTime;
    }

    public async Task<bool> StartMining(Miner miner, int minerSeconds)
    {
        bool flag = await this._minerRepository.Start(miner, minerSeconds);
        return flag;
    }
}