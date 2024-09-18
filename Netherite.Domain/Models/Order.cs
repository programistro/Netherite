namespace Netherite.Domain;

public class Order
{
    private Order(
        Guid id,
        Guid userId,
        Guid currencyPairsId,
        int bet,
        Decimal startPrice,
        DateTime startTime,
        DateTime endTime,
        bool purchaseDirection,
        bool ended)
    {
        this.Id = id;
        this.UserId = userId;
        this.CurrencyPairsId = currencyPairsId;
        this.Bet = bet;
        this.StartPrice = startPrice;
        this.StartTime = startTime;
        this.EndTime = endTime;
        this.PurchaseDirection = purchaseDirection;
        this.Ended = ended;
    }

    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid CurrencyPairsId { get; set; }

    public int Bet { get; set; }

    public Decimal StartPrice { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public bool PurchaseDirection { get; set; }

    public bool Ended { get; set; }

    public static Order Create(
        Guid id,
        Guid userId,
        Guid currencyPairsId,
        int bet,
        Decimal startPrice,
        DateTime startTime,
        DateTime endTime,
        bool purchaseDirection,
        bool ended)
    {
        return new Order(id, userId, currencyPairsId, bet, startPrice, startTime, endTime, purchaseDirection, ended);
    }
}