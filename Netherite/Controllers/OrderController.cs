using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netherite.Contracts;
using Netherite.Domain;
using Netherite.Interface;
using Netherite.Service;

namespace Netherite.Controllers;

[Authorize]
[ApiController]
  [Route("[controller]")]
  public class OrderController : ControllerBase
  {
    private readonly IOrderServices _orderServices;
    private readonly OrderBackgroundService _orderBackgroundService;

    public OrderController(
      IOrderServices orderServices,
      OrderBackgroundService orderBackgroundService)
    {
      this._orderServices = orderServices;
      this._orderBackgroundService = orderBackgroundService;
    }

    [HttpPost("add/{userId}/{interval}/{interestRate}")]
    public async Task<ActionResult<Guid>> CreateOrder(
      Guid userId,
      int interval,
      Decimal interestRate,
      [FromBody] OrderRequset request)
    {
      try
      {
        Order order = Order.Create(Guid.NewGuid(), userId, request.CurrencyPairsId, request.Bet, 0M, DateTime.UtcNow, DateTime.UtcNow, request.PurchaseDirection, false);
        DateTime order1 = await this._orderServices.CreateOrder(userId, interval, order);
        this._orderBackgroundService.ProcessOrderAsync(order.Id, interval, interestRate);
        return Ok(order.Id);
      }
      catch (Exception ex)
      {
        return (ActionResult) this.BadRequest(ex.Message);
      }
    }

    /// <summary>Получение списка ордеров</summary>
    /// <param name="userId">ID пользователя</param>
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<OrderResponse>>> GetOrdersByUserId(Guid userId)
    {
      try
      {
        List<Order> orders = await this._orderServices.GetOrders(userId);
        if (orders == null)
          return NotFound();
        IEnumerable<OrderResponse> response = orders.Select(t => new OrderResponse(t.Id, t.UserId, t.CurrencyPairsId, t.Bet, t.StartPrice, t.StartTime, t.EndTime, t.PurchaseDirection, t.Ended));
        return Ok(response);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }