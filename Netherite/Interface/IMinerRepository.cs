using Netherite.Domain;

namespace Netherite.Interface;

public interface IMinerRepository
{
    Task<bool> Start(Miner miner, int minerSeconds);

    Task<(double remainingTime, bool isFound)> Get(Guid userId);

    Task<bool> End(Guid userId);
}