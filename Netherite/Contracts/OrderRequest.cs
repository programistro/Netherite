namespace Netherite.Contracts;

public record OrderRequset(Guid UserId, Guid CurrencyPairsId, int Bet, bool PurchaseDirection);