using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Netherite.Contracts;
using Netherite.Data;
using Netherite.Domain;
using Netherite.Domain.Entitys;
using Netherite.Interface;

namespace Netherite.Repository;

public class OrderRepository : IOrderRepository
{
  private readonly NetheriteDbContext _context;
  private readonly HttpClient _httpClient;
  private readonly IReferalBonusesServices _referalBonusesServices;

  public OrderRepository(
    NetheriteDbContext context,
    HttpClient httpClient,
    IReferalBonusesServices referalBonusesServices)
  {
    this._context = context;
    this._httpClient = httpClient;
    this._referalBonusesServices = referalBonusesServices;
  }

  public async Task<DateTime> CreateOrder(Guid userId, int interval, Order order)
  {
    UserEntity userEntity1 = await this._context.Users.FindAsync((object) userId);
    UserEntity userEntity2 = userEntity1 ?? throw new Exception("User not found");
    userEntity1 = (UserEntity) null;
    CurrencyPairsEntity currencyPairsEntity = await this._context.CurrencyPairs.FindAsync((object) order.CurrencyPairsId);
    CurrencyPairsEntity currencyPairEntity = currencyPairsEntity ?? throw new Exception("Currency pair not found");
    currencyPairsEntity = (CurrencyPairsEntity) null;
    if (userEntity2.Balance < (Decimal) order.Bet)
      throw new Exception("Недостаточно баланса");
    userEntity2.Balance -= (Decimal) order.Bet;
    string currencyPairSymbol = currencyPairEntity.Name.Replace("/", "");
    HttpResponseMessage response = await this._httpClient.GetAsync("https://api.binance.com/api/v3/ticker/price?symbol=" + currencyPairSymbol);
    if (!response.IsSuccessStatusCode)
      throw new Exception("Не удалось получить начальную цену с Binance");
    Stream utf8Json = await response.Content.ReadAsStreamAsync();
    BinancePriceResponse binancePriceResponse = await JsonSerializer.DeserializeAsync<BinancePriceResponse>(utf8Json);
    utf8Json = (Stream) null;
    BinancePriceResponse priceData = binancePriceResponse;
    binancePriceResponse = (BinancePriceResponse) null;
    Console.WriteLine((object) priceData);
    order.StartPrice = Decimal.Parse(priceData.price, (IFormatProvider) CultureInfo.InvariantCulture);
    DateTime startTime = order.StartTime;
    DateTime endTime = startTime.AddSeconds((double) interval);
    OrderEntity orderEntity = new OrderEntity()
    {
      Id = order.Id,
      UserId = userId,
      CurrencyPairsId = order.CurrencyPairsId,
      Bet = order.Bet,
      StartPrice = order.StartPrice,
      StartTime = startTime,
      EndTime = endTime,
      PurchaseDirection = order.PurchaseDirection,
      Ended = false
    };
    EntityEntry<OrderEntity> entityEntry = await this._context.Orders.AddAsync(orderEntity);
    this._context.Users.Update(userEntity2);
    int num = await this._context.SaveChangesAsync();
    DateTime order1 = endTime;
    userEntity2 = (UserEntity) null;
    currencyPairEntity = (CurrencyPairsEntity) null;
    currencyPairSymbol = (string) null;
    response = (HttpResponseMessage) null;
    priceData = (BinancePriceResponse) null;
    orderEntity = (OrderEntity) null;
    return order1;
  }

  public async System.Threading.Tasks.Task CompleteOrderAfterDelay(
    Guid orderId,
    Decimal interestRate)
  {
    OrderEntity orderEntity = await this._context.Orders.FindAsync((object) orderId);
    if (orderEntity != null)
    {
      Console.WriteLine("Сделка получена");
      UserEntity user = await this._context.Users.FindAsync((object) orderEntity.UserId);
      Console.WriteLine("Пользователь получен");
      CurrencyPairsEntity currencyPairEntity = await this._context.CurrencyPairs.FindAsync((object) orderEntity.CurrencyPairsId);
      if (currencyPairEntity == null)
      {
        orderEntity.Ended = true;
        user.Balance += (Decimal) orderEntity.Bet;
        int num = await this._context.SaveChangesAsync();
        Console.WriteLine("Валютная пара не найдена");
        orderEntity = (OrderEntity) null;
      }
      else
      {
        Console.WriteLine("Валютная пара найдена");
        string currencyPairSymbol = currencyPairEntity.Name.Replace("/", "");
        HttpResponseMessage response = await this._httpClient.GetAsync("https://api.binance.com/api/v3/ticker/price?symbol=" + currencyPairSymbol);
        if (!response.IsSuccessStatusCode)
        {
          orderEntity.Ended = true;
          user.Balance += (Decimal) orderEntity.Bet;
          int num = await this._context.SaveChangesAsync();
          orderEntity = (OrderEntity) null;
        }
        else
        {
          Stream utf8Json = await response.Content.ReadAsStreamAsync();
          BinancePriceResponse binancePriceResponse = await JsonSerializer.DeserializeAsync<BinancePriceResponse>(utf8Json);
          utf8Json = (Stream) null;
          BinancePriceResponse priceData = binancePriceResponse;
          binancePriceResponse = (BinancePriceResponse) null;
          Decimal currentPrice = Decimal.Parse(priceData.price, (IFormatProvider) CultureInfo.InvariantCulture);
          if (orderEntity.PurchaseDirection && currentPrice > orderEntity.StartPrice)
            await this.HandleWinningOrder(orderEntity, user, interestRate, true);
          else if (!orderEntity.PurchaseDirection && currentPrice < orderEntity.StartPrice)
            await this.HandleWinningOrder(orderEntity, user, interestRate, false);
          else if (currentPrice == orderEntity.StartPrice)
          {
            user.Balance += (Decimal) orderEntity.Bet;
            orderEntity.Ended = true;
            int num = await this._context.SaveChangesAsync();
            Console.WriteLine("Ставка ничья сыграла");
          }
          else
            await this.HandleLosingOrder(orderEntity, user);
          user = (UserEntity) null;
          currencyPairEntity = (CurrencyPairsEntity) null;
          currencyPairSymbol = (string) null;
          response = (HttpResponseMessage) null;
          priceData = (BinancePriceResponse) null;
          orderEntity = (OrderEntity) null;
        }
      }
    }
    else
    {
      Console.WriteLine("Сделка не найдена");
      orderEntity = (OrderEntity) null;
    }
  }

  private async System.Threading.Tasks.Task HandleWinningOrder(
    OrderEntity orderEntity,
    UserEntity user,
    Decimal interestRate,
    bool isUpward)
  {
    Decimal profitFactor = interestRate / 100M;
    Decimal profit = (Decimal) orderEntity.Bet * profitFactor;
    Decimal winnings = (Decimal) orderEntity.Bet + profit;
    user.Balance += winnings;
    await this.ProcessReferralBonuses(user, profit);
    orderEntity.Ended = true;
    int num = await this._context.SaveChangesAsync();
    Console.WriteLine(isUpward ? "Ставка вверх сыграла" : "Ставка вниз сыграла");
  }

  private async System.Threading.Tasks.Task HandleLosingOrder(
    OrderEntity orderEntity,
    UserEntity user)
  {
    await this.ProcessReferralBonuses(user, (Decimal) orderEntity.Bet);
    orderEntity.Ended = true;
    int num = await this._context.SaveChangesAsync();
    Console.WriteLine("Ставка не сыграла");
  }

  private async System.Threading.Tasks.Task ProcessReferralBonuses(UserEntity user, Decimal amount)
  {
    UserEntity referrer = await this._context.Users.FirstOrDefaultAsync<UserEntity>((Expression<Func<UserEntity, bool>>) (u => (Guid?) u.Id == user.InvitedId));
    UserEntity referrersReferrer = (UserEntity) null;
    if (referrer != null)
      referrersReferrer = await this._context.Users.FirstOrDefaultAsync<UserEntity>((Expression<Func<UserEntity, bool>>) (u => (Guid?) u.Id == referrer.InvitedId));
    (int, int) valueTuple1 = await this._referalBonusesServices.Execute(user.IsPremium, (int) amount);
    (int, int) valueTuple2 = valueTuple1;
    int referrerReward = valueTuple2.Item1;
    int referrersReferrerReward = valueTuple2.Item2;
    if (referrer == null)
    {
      referrersReferrer = (UserEntity) null;
    }
    else
    {
      referrer.Balance += (Decimal) referrerReward;
      user.Profit += referrerReward;
      if (referrersReferrer != null)
      {
        referrersReferrer.Balance += (Decimal) referrersReferrerReward;
        referrer.Profit += referrersReferrerReward;
      }
      referrersReferrer = (UserEntity) null;
    }
  }

  public async Task<List<Order>> GetOrders(Guid userId)
  {
    List<OrderEntity> orderEntities = await this._context.Orders.Where<OrderEntity>((Expression<Func<OrderEntity, bool>>) (o => o.UserId == userId)).ToListAsync<OrderEntity>();
    List<Order> orders = orderEntities.Select<OrderEntity, Order>((Func<OrderEntity, Order>) (o => Order.Create(o.Id, o.UserId, o.CurrencyPairsId, o.Bet, o.StartPrice, o.StartTime, o.EndTime, o.PurchaseDirection, o.Ended))).ToList<Order>();
    List<Order> orders1 = orders;
    orderEntities = (List<OrderEntity>) null;
    orders = (List<Order>) null;
    return orders1;
  }
}
