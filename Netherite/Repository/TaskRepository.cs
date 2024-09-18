using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Data;
using Netherite.Domain.Entitys;
using Netherite.Interface;

namespace Netherite.Repository;

public class TasksRepository : ITasksRepository
  {
    private readonly NetheriteDbContext _context;

    public TasksRepository(NetheriteDbContext context) => this._context = context;

    public async Task<List<Netherite.Domain.Task>> Get()
    {
      List<TaskEntity> taskEntities = await this._context.Tasks.AsNoTracking<TaskEntity>().ToListAsync<TaskEntity>();
      List<Netherite.Domain.Task> tasks = taskEntities.Select<TaskEntity, Netherite.Domain.Task>((Func<TaskEntity, Netherite.Domain.Task>) (t => Netherite.Domain.Task.Create(t.Id, t.Title, t.Description, t.Icon, t.Reward).Task)).ToList<Netherite.Domain.Task>();
      List<Netherite.Domain.Task> taskList = tasks;
      taskEntities = (List<TaskEntity>) null;
      tasks = (List<Netherite.Domain.Task>) null;
      return taskList;
    }

    public async Task<Guid> Create(Netherite.Domain.Task task)
    {
      TaskEntity taskEntity = new TaskEntity()
      {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        Icon = task.Icon,
        Reward = task.Reward
      };
      EntityEntry<TaskEntity> entityEntry = await this._context.Tasks.AddAsync(taskEntity);
      int num = await this._context.SaveChangesAsync();
      Guid id = taskEntity.Id;
      taskEntity = (TaskEntity) null;
      return id;
    }

    public async Task<bool> Delete(Guid taskId)
    {
      TaskEntity taskEntity = await this._context.Tasks.FindAsync((object) taskId);
      if (taskEntity == null)
        return false;
      this._context.Tasks.Remove(taskEntity);
      int num = await this._context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Update(Guid taskId, Netherite.Domain.Task task)
    {
      TaskEntity taskEntity = await this._context.Tasks.FindAsync((object) taskId);
      if (taskEntity == null)
        return false;
      taskEntity.Title = task.Title;
      taskEntity.Description = task.Description;
      taskEntity.Icon = task.Icon;
      taskEntity.Reward = task.Reward;
      this._context.Tasks.Update(taskEntity);
      int num = await this._context.SaveChangesAsync();
      return true;
    }
  }