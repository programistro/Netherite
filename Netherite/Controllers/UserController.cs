﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netherite.Contracts;
using Netherite.Data;
using Netherite.Domain;
using Netherite.Domain.Entitys;
using Netherite.Interface;
using Task = Netherite.Domain.Task;

namespace Netherite.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
  private readonly IUserServices _userServices;

  private readonly NetheriteDbContext _context;

  public UserController(IUserServices userServices, NetheriteDbContext context)
  {
    this._userServices = userServices;
    _context = context;
  }

  /// <summary>Получение определенного пользователя</summary>
  /// <param name="userId">ID пользователя</param>
  [HttpGet("{userId}")]
  public async Task<ActionResult<UserResponse>> GetUser(Guid userId)
  {
    try
    {
      Netherite.Domain.User user = await this._userServices.GetUser(userId);
      if (user == null)
        return (ActionResult<UserResponse>)(ActionResult)this.NotFound((object)"Пользователь не найден.");
      UserResponse response = new UserResponse(user.Id, user.Balance, user.Location, user.InvitedId, user.IsPremium,
        user.TelegramId, user.TelegramName, user.Wallet, user.Profit);
      return (ActionResult<UserResponse>)(ActionResult)this.Ok((object)response);
    }
    catch (Exception ex)
    {
      return (ActionResult<UserResponse>)(ActionResult)this.BadRequest((object)ex.Message);
    }
  }

  /// <summary>Получение определенного пользователя по номеру кошелька</summary>
  /// <param name="wallet">Номер кошелька</param>
  [HttpGet("by-wallet/{wallet}")]
  public async Task<ActionResult<UserResponse>> GetUserByWallet(string wallet)
  {
    try
    {
      Netherite.Domain.User user = await this._userServices.GetUserByWallet(wallet);
      if (user == null)
        return (ActionResult<UserResponse>)(ActionResult)this.NotFound((object)"Пользователь не найден.");
      UserResponse response = new UserResponse(user.Id, user.Balance, user.Location, user.InvitedId, user.IsPremium,
        user.TelegramId, user.TelegramName, user.Wallet, user.Profit);
      return (ActionResult<UserResponse>)(ActionResult)this.Ok((object)response);
    }
    catch (Exception ex)
    {
      return (ActionResult<UserResponse>)(ActionResult)this.BadRequest((object)ex.Message);
    }
  }

  /// <summary>Получение доступных пользователю заданий</summary>
  /// <param name="userId">ID пользователя</param>
  [HttpGet("tasks/{userId}")]
  public async Task<ActionResult<List<Netherite.Domain.Task>>> GetAvailableTasks(
    Guid userId)
  {
    try
    {
      List<Netherite.Domain.Task> tasks = await this._userServices.GetAvailableTasks(userId);
      return tasks != null
        ? (ActionResult<List<Netherite.Domain.Task>>)(ActionResult)this.Ok((object)tasks)
        : (ActionResult<List<Netherite.Domain.Task>>)(ActionResult)this.NotFound(
          (object)"Не найдено доступных заданий.");
    }
    catch (Exception ex)
    {
      return (ActionResult<List<Netherite.Domain.Task>>)(ActionResult)this.BadRequest((object)ex.Message);
    }
  }

  /// <summary>Получение рефералов пользователя</summary>
  /// <param name="userId">ID пользователя</param>
  [HttpGet("referals/{userId}")]
  public async Task<ActionResult<Guid>> GetReferals(Guid userId)
  {
    try
    {
      List<object> referals = await this._userServices.GetReferals(userId);
      return (ActionResult<Guid>)(ActionResult)this.Ok((object)referals);
    }
    catch (Exception ex)
    {
      return (ActionResult<Guid>)(ActionResult)this.BadRequest((object)ex.Message);
    }
  }

  /// <summary>Регистрация пользователя</summary>
  /// <param name="request">Запрос на создание пользователя содержит баланс, локацию, ID пригласившего, премиум, телеграм ID, телеграм имя, номер кошелька.</param>
  [HttpPost("register")]
  public async Task<ActionResult<Guid>> RegisterUser([FromBody] UserRequest request)
  {
    try
    {
      (Netherite.Domain.User user, string str) = Netherite.Domain.User.Create(Guid.NewGuid(), 0M, request.Location,
        request.InvitedId, request.IsPremium, request.TelegramId, request.TelegramName, request.Wallet, 0);
      if (!string.IsNullOrEmpty(str))
        return (ActionResult<Guid>)(ActionResult)this.BadRequest((object)str);
      Guid guid = await this._userServices.RegisterUser(user);
      Guid userId = guid;
      guid = new Guid();
      return (ActionResult<Guid>)(ActionResult)this.Ok((object)userId);
    }
    catch (Exception ex)
    {
      return (ActionResult<Guid>)(ActionResult)this.BadRequest((object)ex.Message);
    }
  }

  /// <summary>Выполнение задания</summary>
  /// <param name="userId">ID пользователя</param>
  /// <param name="taskId">ID задания</param>
  [HttpPost("complete/{userId}/{taskId}")]
  public async Task<ActionResult<Guid>> CompleteTask(Guid userId, Guid taskId)
  {
    try
    {
      Guid guid = await this._userServices.CompleteTask(userId, taskId);
      Guid completedTaskId = guid;
      guid = new Guid();
      return (ActionResult<Guid>)(ActionResult)this.Ok((object)completedTaskId);
    }
    catch (Exception ex)
    {
      return (ActionResult<Guid>)(ActionResult)this.BadRequest((object)ex.Message);
    }
  }

  /// <summary>Обновление пользователя</summary>
  /// <param name="userId">ID пользователя</param>
  /// <param name="updatedUserRequest">Запрос на обновление пользователя содержит локацию, ID пригласившего, премиум, телеграм ID, телеграм имя, номер кошелька.</param>
  [HttpPut("update/{userId}")]
  public async Task<ActionResult<UserResponse>> UpdateUser(
    Guid userId,
    [FromBody] UserRequest updatedUserRequest)
  {
    try
    {
      Netherite.Domain.User existingUser = await this._userServices.GetUser(userId);
      if (existingUser == null)
        return (ActionResult<UserResponse>)(ActionResult)this.NotFound((object)"Пользователь не найден.");
      (Netherite.Domain.User user1, string str) = Netherite.Domain.User.Create(userId, existingUser.Balance,
        updatedUserRequest.Location, updatedUserRequest.InvitedId, updatedUserRequest.IsPremium,
        updatedUserRequest.TelegramId, updatedUserRequest.TelegramName, updatedUserRequest.Wallet, existingUser.Profit);
      if (!string.IsNullOrEmpty(str))
        return (ActionResult<UserResponse>)(ActionResult)this.BadRequest((object)str);
      Netherite.Domain.User user = await this._userServices.UpdateUser(userId, user1);
      UserResponse response = new UserResponse(user.Id, user.Balance, user.Location, user.InvitedId, user.IsPremium,
        user.TelegramId, user.TelegramName, user.Wallet, user.Profit);
      return (ActionResult<UserResponse>)(ActionResult)this.Ok((object)response);
    }
    catch (Exception ex)
    {
      return (ActionResult<UserResponse>)(ActionResult)this.BadRequest((object)ex.Message);
    }
  }

  [HttpGet("get-all-users")]
  public async Task<IActionResult> GetAllUser()
  {
    var users = _context.Users;

    return Ok(users);
  }

  [HttpGet("get-all-users-with-prem")]
  public async Task<IActionResult> GetAllPremUser()
  {
    var premUsers = _context.Users.Where(x => x.IsPremium == true);

    return Ok(premUsers);
  }

  [HttpGet("get-balance-user")]
  public async Task<IActionResult> GetBalanceUser(Guid id)
  {
    var user = _context.Users.FirstOrDefault(x => x.Id == id);

    if (user == null)
    {
      return NotFound();
    }

    return Ok(user.Balance);
  }
  
  [HttpGet("get-profit-user")]
  public async Task<IActionResult> GetProfitUser(Guid id)
  {
    var user = _context.Users.FirstOrDefault(x => x.Id == id);

    if (user == null)
    {
      return NotFound();
    }

    return Ok(user.Profit);
  }
}