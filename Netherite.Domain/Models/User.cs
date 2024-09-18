namespace Netherite.Domain;

public class User
{
    private User(
        Guid id,
        Decimal balance,
        string location,
        Guid? invitedId,
        bool isPremium,
        string telegramId,
        string telegramName,
        string wallet,
        int profit)
    {
        this.Id = id;
        this.Balance = balance;
        this.Location = location;
        this.InvitedId = invitedId;
        this.IsPremium = isPremium;
        this.TelegramId = telegramId;
        this.TelegramName = telegramName;
        this.Wallet = wallet;
        this.Profit = profit;
    }

    public Guid Id { get; }

    public Decimal Balance { get; }

    public string Location { get; }

    public Guid? InvitedId { get; } = new Guid?(Guid.Empty);

    public bool IsPremium { get; }

    public string TelegramId { get; }

    public string TelegramName { get; }

    public string Wallet { get; }

    public int Profit { get; }

    public static (User User, string Error) Create(
        Guid id,
        Decimal balance,
        string location,
        Guid? invitedId,
        bool isPremium,
        string telegramId,
        string telegramName,
        string wallet,
        int profit)
    {
        string str = string.Empty;
        if (string.IsNullOrEmpty(location))
            str = "Location can't be empty";
        if (string.IsNullOrEmpty(telegramId))
            str = "TelegramId can't be empty";
        if (string.IsNullOrEmpty(telegramName))
            str = "TelegramName can't be empty";
        if (string.IsNullOrEmpty(wallet))
            str = "Wallet can't be empty";
        return (new User(id, balance, location, invitedId, isPremium, telegramId, telegramName, wallet, profit), str);
    }
}