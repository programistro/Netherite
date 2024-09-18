namespace Netherite.Domain.Entitys;

public class OrderEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid CurrencyPairsId { get; set; }

    public int Bet { get; set; }

    public Decimal StartPrice { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public bool PurchaseDirection { get; set; }

    public bool Ended { get; set; }
}