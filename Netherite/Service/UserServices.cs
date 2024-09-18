using Netherite.Domain;
using Netherite.Interface;

namespace Netherite.Service;

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;

    public UserServices(IUserRepository userRepository) => this._userRepository = userRepository;

    public async Task<User> GetUser(Guid userId)
    {
        User user = await this._userRepository.Get(userId);
        return user;
    }

    public async Task<User?> GetUserByWallet(string wallet)
    {
        User byWallet = await this._userRepository.GetByWallet(wallet);
        return byWallet;
    }

    public async Task<Guid> RegisterUser(User user)
    {
        Guid guid = await this._userRepository.Register(user);
        return guid;
    }

    public async Task<User> UpdateUser(Guid userId, User updatedUser)
    {
        User user = await this._userRepository.Update(userId, updatedUser);
        return user;
    }

    public async Task<List<Netherite.Domain.Task>> GetAvailableTasks(Guid userId)
    {
        List<Netherite.Domain.Task> availableTasks = await this._userRepository.GetAvailableTasks(userId);
        return availableTasks;
    }

    public async Task<Guid> CompleteTask(Guid userId, Guid taskId)
    {
        Guid guid = await this._userRepository.Complete(userId, taskId);
        return guid;
    }

    public async Task<List<object>> GetReferals(Guid userId)
    {
        List<object> referals = await this._userRepository.GetReferals(userId);
        return referals;
    }
}