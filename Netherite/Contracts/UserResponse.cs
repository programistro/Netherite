namespace Netherite.Contracts;

public record UserResponse(
        Guid Id,
        Decimal Balance,
        string Location,
        Guid? InvitedId,
        bool IsPremium,
        string TelegramId,
        string TelegramName,
        string Wallet,
        int Profit)
    ;