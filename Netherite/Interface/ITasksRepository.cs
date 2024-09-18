namespace Netherite.Interface;

public interface ITasksRepository
{
    Task<List<Netherite.Domain.Task>> Get();

    Task<Guid> Create(Netherite.Domain.Task task);

    Task<bool> Delete(Guid taskId);

    Task<bool> Update(Guid taskId, Netherite.Domain.Task task);
}