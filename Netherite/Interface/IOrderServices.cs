using Netherite.Domain;

namespace Netherite.Interface;

public interface IOrderServices
{
    Task<DateTime> CreateOrder(Guid userId, int interval, Order order);

    Task<List<Order>> GetOrders(Guid userId);

    System.Threading.Tasks.Task CompleteOrderAfterDelay(Guid orderId, Decimal interestRate);
}