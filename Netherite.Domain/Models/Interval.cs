namespace Netherite.Domain;

public class Interval
{
    private Interval(Guid id, int time)
    {
        this.Id = id;
        this.Time = time;
    }

    public Guid Id { get; set; }

    public int Time { get; set; }

    public static Interval Create(Guid id, int time)
    {
        string empty = string.Empty;
        return new Interval(id, time);
    }
}