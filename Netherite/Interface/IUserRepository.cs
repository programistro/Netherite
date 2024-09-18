using Netherite.Domain;

namespace Netherite.Interface;

public interface IUserRepository
{
    Task<User> Get(Guid userId);

    Task<User?> GetByWallet(string wallet);

    Task<Guid> Register(User user);

    Task<User> Update(Guid userId, User updatedUser);

    Task<List<Netherite.Domain.Task>> GetAvailableTasks(Guid userId);

    Task<Guid> Complete(Guid userId, Guid taskId);

    Task<List<object>> GetReferals(Guid userId);
}