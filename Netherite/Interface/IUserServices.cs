using Netherite.Domain;

namespace Netherite.Interface;

public interface IUserServices
{
    Task<User> GetUser(Guid userId);

    Task<User?> GetUserByWallet(string wallet);

    Task<Guid> RegisterUser(User user);

    Task<User> UpdateUser(Guid userId, User updatedUser);

    Task<List<Netherite.Domain.Task>> GetAvailableTasks(Guid userId);

    Task<Guid> CompleteTask(Guid userId, Guid taskId);

    Task<List<object>> GetReferals(Guid userId);
}