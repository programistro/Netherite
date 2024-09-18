namespace Netherite.Contracts;

public record UserRequest(
        string Location,
        Guid? InvitedId,
        bool IsPremium,
        string TelegramId,
        string TelegramName,
        string Wallet)
    ;