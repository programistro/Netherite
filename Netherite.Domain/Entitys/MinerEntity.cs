namespace Netherite.Domain.Entitys;

public class MinerEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public int Reward { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}