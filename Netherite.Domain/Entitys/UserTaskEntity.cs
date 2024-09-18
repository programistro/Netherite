namespace Netherite.Domain.Entitys;

public class UserTaskEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public UserEntity? User { get; set; } = (UserEntity) null;

    public Guid TaskId { get; set; }

    public TaskEntity? Task { get; set; } = (TaskEntity) null;
}