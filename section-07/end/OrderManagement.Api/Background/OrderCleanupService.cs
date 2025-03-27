using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Data;

namespace OrderManagement.Api.Background;

public class OrderCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderCleanupService> _logger;
    
    public OrderCleanupService(
        IServiceProvider serviceProvider,
        ILogger<OrderCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
                
                _logger.LogInformation("Running order cleanup process");
                
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                
                var abandonedDate = DateTime.UtcNow.AddDays(-7);
                var cartItems = await dbContext.CartItems
                    .Where(ci => ci.LastUpdated < abandonedDate)
                    .ToListAsync(stoppingToken);
                
                dbContext.CartItems.RemoveRange(cartItems);
                
                await dbContext.SaveChangesAsync(stoppingToken);
                
                _logger.LogInformation($"Removed {cartItems.Count} abandoned cart items");
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error during order cleanup process");
            }
        }
    }
}
