using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Data;
using Netherite.Domain;
using Netherite.Domain.Entitys;
using Netherite.Interface;

namespace Netherite.Repository;

  public class UserRepository : IUserRepository
  {
    private readonly NetheriteDbContext _context;
    private readonly IReferalBonusesServices _referalBonusesServices;

    public UserRepository(
      NetheriteDbContext context,
      IReferalBonusesServices referalBonusesServices)
    {
      this._context = context;
      this._referalBonusesServices = referalBonusesServices;
    }

    public async Task<User> Get(Guid userId)
    {
      UserEntity userEntity = await this._context.Users.FindAsync((object) userId);
      User user = userEntity != null ? this.MapToDomain(userEntity) : throw new Exception("Пользователь не найден.");
      userEntity = (UserEntity) null;
      return user;
    }

    public async Task<Guid> Register(User user)
    {
      UserEntity existingUser = await this._context.Users.FirstOrDefaultAsync<UserEntity>((Expression<Func<UserEntity, bool>>) (u => u.TelegramId == user.TelegramId));
      if (existingUser != null)
        return existingUser.Id;
      UserEntity userEntity = this.MapToEntity(user);
      EntityEntry<UserEntity> entityEntry = await this._context.Users.AddAsync(userEntity);
      int num = await this._context.SaveChangesAsync();
      return userEntity.Id;
    }

    public async Task<User> Update(Guid userId, User updatedUser)
    {
      UserEntity userEntity1 = await this._context.Users.FindAsync((object) userId);
      UserEntity userEntity2 = userEntity1 ?? throw new Exception("Пользователь не найден.");
      userEntity1 = (UserEntity) null;
      userEntity2.Balance = updatedUser.Balance;
      userEntity2.Location = updatedUser.Location;
      userEntity2.InvitedId = updatedUser.InvitedId;
      userEntity2.IsPremium = updatedUser.IsPremium;
      userEntity2.TelegramId = updatedUser.TelegramId;
      userEntity2.TelegramName = updatedUser.TelegramName;
      userEntity2.Wallet = updatedUser.Wallet;
      int num = await this._context.SaveChangesAsync();
      User domain = this.MapToDomain(userEntity2);
      userEntity2 = (UserEntity) null;
      return domain;
    }

    public async Task<List<Netherite.Domain.Task>> GetAvailableTasks(Guid userId)
    {
      List<Guid> userCompletedTaskIds = await this._context.UserTasks.AsNoTracking<UserTaskEntity>().Where<UserTaskEntity>((Expression<Func<UserTaskEntity, bool>>) (ut => ut.UserId == userId)).Select<UserTaskEntity, Guid>((Expression<Func<UserTaskEntity, Guid>>) (ut => ut.TaskId)).ToListAsync<Guid>();
      List<TaskEntity> availableTasksEntities = await this._context.Tasks.Where<TaskEntity>((Expression<Func<TaskEntity, bool>>) (t => !userCompletedTaskIds.Contains(t.Id))).ToListAsync<TaskEntity>();
      List<Netherite.Domain.Task> availableTasks = new List<Netherite.Domain.Task>();
      foreach (TaskEntity entity in availableTasksEntities)
      {
        (Netherite.Domain.Task Task, string Error) = Netherite.Domain.Task.Create(entity.Id, entity.Title, entity.Description, entity.Icon, entity.Reward);
        if (string.IsNullOrEmpty(Error))
          availableTasks.Add(Task);
        else
          Console.WriteLine("Ошибка создания задачи: " + Error);
        Task = (Netherite.Domain.Task) null;
        Error = (string) null;
      }
      List<Netherite.Domain.Task> availableTasks1 = availableTasks;
      availableTasksEntities = (List<TaskEntity>) null;
      availableTasks = (List<Netherite.Domain.Task>) null;
      return availableTasks1;
    }

    public async Task<Guid> Complete(Guid userId, Guid taskId)
    {
      UserEntity userEntity = await this._context.Users.FindAsync((object) userId);
      UserEntity user = userEntity ?? throw new Exception("Пользователь не найден.");
      userEntity = (UserEntity) null;
      TaskEntity taskEntity = await this._context.Tasks.FindAsync((object) taskId);
      TaskEntity task = taskEntity ?? throw new Exception("Задание не найдено.");
      taskEntity = (TaskEntity) null;
      UserTaskEntity existingUserTaskEntity = await this._context.UserTasks.FirstOrDefaultAsync<UserTaskEntity>((Expression<Func<UserTaskEntity, bool>>) (ut => ut.UserId == userId && ut.TaskId == taskId));
      if (existingUserTaskEntity != null)
        throw new Exception("Задание данным пользователем уже выполнено.");
      UserTaskEntity userTaskEntity = new UserTaskEntity()
      {
        Id = Guid.NewGuid(),
        UserId = userId,
        User = user,
        TaskId = taskId,
        Task = task
      };
      EntityEntry<UserTaskEntity> entityEntry = await this._context.UserTasks.AddAsync(userTaskEntity);
      int reward = task.Reward;
      user.Balance += (Decimal) reward;
      UserEntity referrer = await this._context.Users.FirstOrDefaultAsync<UserEntity>((Expression<Func<UserEntity, bool>>) (u => (Guid?) u.Id == user.InvitedId));
      UserEntity referrersReferrer = (UserEntity) null;
      if (referrer != null)
        referrersReferrer = await this._context.Users.FirstOrDefaultAsync<UserEntity>((Expression<Func<UserEntity, bool>>) (u => (Guid?) u.Id == referrer.InvitedId));
      (int, int) valueTuple1 = await this._referalBonusesServices.Execute(user.IsPremium, reward);
      (int, int) valueTuple2 = valueTuple1;
      int referrerReward = valueTuple2.Item1;
      int referrersReferrerReward = valueTuple2.Item2;
      if (referrer != null)
      {
        referrer.Balance += (Decimal) referrerReward;
        user.Profit += referrerReward;
        if (referrersReferrer != null)
        {
          referrersReferrer.Balance += (Decimal) referrersReferrerReward;
          referrer.Profit += referrersReferrerReward;
        }
      }
      int num = await this._context.SaveChangesAsync();
      Guid id = userTaskEntity.Id;
      task = (TaskEntity) null;
      existingUserTaskEntity = (UserTaskEntity) null;
      userTaskEntity = (UserTaskEntity) null;
      referrersReferrer = (UserEntity) null;
      return id;
    }

    public async Task<List<object>> GetReferals(Guid userId)
    {
      List<UserEntity> users = await this._context.Users.AsNoTracking<UserEntity>().Where<UserEntity>((Expression<Func<UserEntity, bool>>) (ut => ut.InvitedId == (Guid?) userId)).ToListAsync<UserEntity>();
      List<object> userDtos = new List<object>();
      foreach (UserEntity userEntity in users)
      {
        UserEntity entity = userEntity;
        int referalsCount = await this._context.Users.AsNoTracking<UserEntity>().Where<UserEntity>((Expression<Func<UserEntity, bool>>) (ut => ut.InvitedId == (Guid?) entity.Id)).CountAsync<UserEntity>();
        var userDto = new
        {
          id = entity.Id.ToString(),
          profit = entity.Profit,
          referals = referalsCount,
          telegramName = entity.TelegramName
        };
        userDtos.Add((object) userDto);
        userDto = null;
      }
      List<object> referals = userDtos;
      users = (List<UserEntity>) null;
      userDtos = (List<object>) null;
      return referals;
    }

    public async Task<User?> GetByWallet(string wallet)
    {
      UserEntity userEntity = await this._context.Users.FirstOrDefaultAsync<UserEntity>((Expression<Func<UserEntity, bool>>) (u => u.Wallet == wallet));
      User domain = userEntity != null ? this.MapToDomain(userEntity) : (User) null;
      userEntity = (UserEntity) null;
      return domain;
    }

    private Domain.User MapToDomain(UserEntity userEntity)
    {
      (Domain.User User, string Error) = Domain.User.Create(userEntity.Id, userEntity.Balance, userEntity.Location, userEntity.InvitedId, userEntity.IsPremium, userEntity.TelegramId, userEntity.TelegramName, userEntity.Wallet, userEntity.Profit);
      if (!string.IsNullOrEmpty(Error))
        throw new Exception("User mapping error: " + Error);
      return User;
    }

    private UserEntity MapToEntity(Domain.User user)
    {
      return new UserEntity()
      {
        Id = user.Id,
        Balance = user.Balance,
        Location = user.Location,
        InvitedId = user.InvitedId,
        IsPremium = user.IsPremium,
        TelegramId = user.TelegramId,
        TelegramName = user.TelegramName,
        Wallet = user.Wallet,
        Profit = user.Profit
      };
    }
  }