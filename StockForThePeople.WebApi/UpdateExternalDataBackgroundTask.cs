
using StockForThePeople.ExternalData;

namespace StockForThePeople.WebApi;

public class UpdateExternalDataBackgroundTask : IHostedService, IDisposable
{
    private PeriodicTimer _timer;
    private IServiceScopeFactory _serviceScopeFactory;
    private ILogger<UpdateExternalDataBackgroundTask> _logger;

    public UpdateExternalDataBackgroundTask(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<UpdateExternalDataBackgroundTask> logger
        )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        UpdateExternalData();
        return Task.CompletedTask;
    }

    private async Task UpdateExternalData()
    {
        while(await _timer.WaitForNextTickAsync())
        {
            _logger.LogInformation("{var} - {var2} ran at: {var3}. Remember once I start to host this somewhere, I can change this to daily instead of once at startup.", 
                nameof(UpdateExternalDataBackgroundTask),
                nameof(UpdateExternalData), 
                DateTime.Now.ToLongTimeString());

            // https://www.youtube.com/watch?v=FSjCGdkbiCA
            // brief solution to when you want to use scoped services inside a singleton service

            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            IExternalDataService externalDataService = scope.ServiceProvider.GetRequiredService<IExternalDataService>();
            await externalDataService.UpdateDataAsync();

            // once the application is hosted somewhere, if ever, this needs to change to 24 hours.
            _timer.Period = Timeout.InfiniteTimeSpan;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Period = Timeout.InfiniteTimeSpan;
        return Task.CompletedTask;
    }
    public void Dispose()
    {
        _timer.Dispose();
    }
}
