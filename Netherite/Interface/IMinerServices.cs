using Netherite.Domain;

namespace Netherite.Interface;

public interface IMinerServices
{
    Task<bool> StartMining(Miner miner, int minerSeconds);

    Task<(double remainingTime, bool isFound)> GetCurrentTime(Guid userId);

    Task<bool> EndMining(Guid userId);
}