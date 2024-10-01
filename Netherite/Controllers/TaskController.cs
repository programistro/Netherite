using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netherite.Contracts;
using Netherite.Interface;

namespace Netherite.Controllers;

[Authorize]
[ApiController]
  [Route("[controller]")]
  public class TasksController : ControllerBase
  {
    private readonly ITasksServices _tasksServices;

    public TasksController(ITasksServices tasksServices) => this._tasksServices = tasksServices;

    /// <summary>Получение списка заданий</summary>
    [HttpGet]
    public async Task<ActionResult<List<TasksResponse>>> GetTasks()
    {
      try
      {
        List<Netherite.Domain.Task> tasks = await this._tasksServices.GetAllTasks();
        IEnumerable<TasksResponse> response = tasks.Select<Netherite.Domain.Task, TasksResponse>((Func<Netherite.Domain.Task, TasksResponse>) (t => new TasksResponse(t.Id, t.Title, t.Description, t.Icon, t.Reward)));
        return (ActionResult<List<TasksResponse>>) (ActionResult) this.Ok((object) response);
      }
      catch (Exception ex)
      {
        return (ActionResult<List<TasksResponse>>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }

    /// <summary>Создание задания</summary>
    /// <param name="request">Запрос на создание задания содержит заголовок, описание, иконку, награду.</param>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTask([FromBody] TasksRequest request)
    {
      try
      {
        (Netherite.Domain.Task task, string Error) = Netherite.Domain.Task.Create(Guid.NewGuid(), request.Title, request.Description, request.Icon, request.Reward);
        if (!string.IsNullOrEmpty(Error))
          return (ActionResult<Guid>) (ActionResult) this.NotFound((object) Error);
        Guid guid = await this._tasksServices.CreateTask(task);
        Guid taskId = guid;
        guid = new Guid();
        return (ActionResult<Guid>) (ActionResult) this.Ok((object) taskId);
      }
      catch (Exception ex)
      {
        return (ActionResult<Guid>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }

    /// <summary>Обновление данных задания</summary>
    /// <param name="taskId">ID задания</param>
    /// <param name="request">Запрос на обновление задания содержит заголовок, описание, иконку, награду.</param>
    [HttpPut("/Tasks/{taskId}")]
    public async Task<ActionResult<bool>> UpdateTask(Guid taskId, [FromBody] TasksRequest request)
    {
      try
      {
        (Netherite.Domain.Task task, string str) = Netherite.Domain.Task.Create(taskId, request.Title, request.Description, request.Icon, request.Reward);
        if (!string.IsNullOrEmpty(str))
          return (ActionResult<bool>) (ActionResult) this.BadRequest((object) str);
        bool result = await this._tasksServices.UpdateTask(taskId, task);
        return result ? (ActionResult<bool>) (ActionResult) this.Ok((object) result) : (ActionResult<bool>) (ActionResult) this.NotFound((object) "Task not found or could not be updated.");
      }
      catch (Exception ex)
      {
        return (ActionResult<bool>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }

    /// <summary>Удаление задания</summary>
    /// <param name="taskId">ID задания</param>
    [HttpDelete("/Tasks/{taskId}")]
    public async Task<ActionResult<bool>> DeleteTask(Guid taskId)
    {
      try
      {
        bool result = await this._tasksServices.DeleteTask(taskId);
        return result ? (ActionResult<bool>) (ActionResult) this.Ok((object) result) : (ActionResult<bool>) (ActionResult) this.NotFound((object) "Task not found or could not be updated.");
      }
      catch (Exception ex)
      {
        return (ActionResult<bool>) (ActionResult) this.BadRequest((object) ex.Message);
      }
    }
  }