using Netherite.Domain;
using Netherite.Interface;

namespace Netherite.Service;

public class OrderService : IOrderServices
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        this._orderRepository = orderRepository;
    }

    public Task<DateTime> CreateOrder(Guid userId, int interval, Order order)
    {
        return this._orderRepository.CreateOrder(userId, interval, order);
    }

    public Task<List<Order>> GetOrders(Guid userId) => this._orderRepository.GetOrders(userId);

    public System.Threading.Tasks.Task CompleteOrderAfterDelay(Guid orderId, Decimal interestRate)
    {
        return this._orderRepository.CompleteOrderAfterDelay(orderId, interestRate);
    }
}