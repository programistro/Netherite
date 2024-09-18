namespace Netherite.Interface;

public interface IReferalBonusesServices
{
    Task<(int referrerReward, int referrersReferrerReward)> Execute(bool isPremium, int reward);
}