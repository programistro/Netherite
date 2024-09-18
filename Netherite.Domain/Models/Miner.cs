namespace Netherite.Domain;

public class Miner
{
    private Miner(Guid id, Guid userId, int reward, DateTime startTime, DateTime endTime)
    {
        this.Id = id;
        this.UserId = userId;
        this.Reward = reward;
        this.StartTime = startTime;
        this.EndTime = endTime;
    }

    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public int Reward { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public static Miner Create(
        Guid id,
        Guid userId,
        int reward,
        DateTime startTime,
        DateTime endTime)
    {
        return new Miner(id, userId, reward, startTime, endTime);
    }
}