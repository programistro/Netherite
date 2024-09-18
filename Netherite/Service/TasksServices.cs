using Netherite.Interface;

namespace Netherite.Service;

public class TasksServices : ITasksServices
{
    private readonly ITasksRepository _taskRepository;

    public TasksServices(ITasksRepository tasksRepository)
    {
        this._taskRepository = tasksRepository;
    }

    public async Task<List<Netherite.Domain.Task>> GetAllTasks()
    {
        List<Netherite.Domain.Task> allTasks = await this._taskRepository.Get();
        return allTasks;
    }

    public async Task<Guid> CreateTask(Netherite.Domain.Task task)
    {
        Guid task1 = await this._taskRepository.Create(task);
        return task1;
    }

    public async Task<bool> DeleteTask(Guid taskId)
    {
        bool flag = await this._taskRepository.Delete(taskId);
        return flag;
    }

    public async Task<bool> UpdateTask(Guid taskId, Netherite.Domain.Task task)
    {
        bool flag = await this._taskRepository.Update(taskId, task);
        return flag;
    }
}