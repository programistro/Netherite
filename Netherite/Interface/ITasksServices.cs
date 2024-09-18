namespace Netherite.Interface;

public interface ITasksServices
{
    Task<List<Netherite.Domain.Task>> GetAllTasks();

    Task<Guid> CreateTask(Netherite.Domain.Task task);

    Task<bool> DeleteTask(Guid taskId);

    Task<bool> UpdateTask(Guid taskId, Netherite.Domain.Task task);
}