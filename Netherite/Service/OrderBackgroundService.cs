using Netherite.Interface;

namespace Netherite.Service;

public class OrderBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public OrderBackgroundService(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(-1, stoppingToken);
    }

    public async Task ProcessOrderAsync(Guid orderId, int delayInSeconds, Decimal interestRate)
    {
        using (IServiceScope scope = this._serviceProvider.CreateScope())
        {
            IOrderServices orderServices = scope.ServiceProvider.GetRequiredService<IOrderServices>();
            await Task.Delay(delayInSeconds * 1000);
            await orderServices.CompleteOrderAfterDelay(orderId, interestRate);
            orderServices = (IOrderServices) null;
        }
    }
}