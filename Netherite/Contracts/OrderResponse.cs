namespace Netherite.Contracts;

public record OrderResponse(
        Guid Id,
        Guid UserId,
        Guid CurrencyPairsId,
        int Bet,
        Decimal StartPrice,
        DateTime StartTime,
        DateTime EndTime,
        bool PurchaseDirection,
        bool Ended)
    ;