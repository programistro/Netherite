namespace Netherite.Domain;

public class CurrencyPairs
{
    private CurrencyPairs(
        Guid id,
        string name,
        string nameTwo,
        string icon,
        Decimal interestRate)
    {
        this.Id = id;
        this.Name = name;
        this.NameTwo = nameTwo;
        this.Icon = icon;
        this.InterestRate = interestRate;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? NameTwo { get; set; }

    public string? Icon { get; set; }

    public Decimal InterestRate { get; set; }

    public static CurrencyPairs Create(
        Guid id,
        string name,
        string nameTwo,
        string icon,
        Decimal interestRate)
    {
        return new CurrencyPairs(id, name, nameTwo, icon, interestRate);
    }
}