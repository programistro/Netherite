namespace Netherite.Contracts;

public record CurrencyPairsResponse(
        Guid Id,
        string Name,
        string NameTwo,
        string Icon,
        Decimal InterestRate)
    ;