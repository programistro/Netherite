using Netherite.Interface;

namespace Netherite.Service;

public class ReferalBonusesServices : IReferalBonusesServices
{
    public async Task<(int referrerReward, int referrersReferrerReward)> Execute(
        bool isPremium,
        int reward)
    {
        double referrerBonusPercentage = isPremium ? 0.2 : 0.1;
        int referrerReward = (int) ((double) reward * referrerBonusPercentage);
        double referrersReferrerBonusPercentage = isPremium ? 0.06 : 0.03;
        int referrersReferrerReward = (int) ((double) reward * referrersReferrerBonusPercentage);
        (int, int) valueTuple = await Task.FromResult<(int, int)>((referrerReward, referrersReferrerReward));
        return valueTuple;
    }
}