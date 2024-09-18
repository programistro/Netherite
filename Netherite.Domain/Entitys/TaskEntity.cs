namespace Netherite.Domain.Entitys;

public class TaskEntity
{
    public Guid Id { get; set; }

    public string Title { get; set; } = (string) null;

    public string Description { get; set; } = (string) null;

    public string Icon { get; set; } = (string) null;

    public int Reward { get; set; } = 0;

    public ICollection<UserTaskEntity> UserTasks { get; set; } = (ICollection<UserTaskEntity>) new List<UserTaskEntity>();
}