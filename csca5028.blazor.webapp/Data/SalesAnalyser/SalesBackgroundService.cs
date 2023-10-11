using csca5028.lib;

namespace csca5028.blazor.webapp.Data.SalesAnalyser
{
    public class SalesBackgroundService : BackgroundService, IDisposable
    {
        private readonly ILogger<SalesBackgroundService> _logger;
        private readonly ISalesAnalyzer _salesAnalyser;
        private readonly ISalesAnalyzer _cache;
        private readonly int _intervalInMinutes = 1;
        private Timer _timer;

        public SalesBackgroundService(ILogger<SalesBackgroundService> logger, ISalesAnalyzer cache)
        {
            _logger = logger;
            _salesAnalyser = new SalesAnalyzer();
            _cache = cache;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SalesBackgroundService is starting.");
            if(!cancellationToken.IsCancellationRequested)
            {
                RefreshCache(cancellationToken);
                _timer = new Timer(RefreshCache, cancellationToken, 0, _intervalInMinutes * 60000);
            }

            return Task.CompletedTask;
        }

        private async void RefreshCache(object state)
        {
            _logger.LogInformation("SalesBackgroundService is working.");
            CancellationToken token = (CancellationToken)state;
            if (!token.IsCancellationRequested)
            {   
                await _cache.SetSalesPerformance(await _salesAnalyser.LoadSalesPerformance());
                await _cache.SetStoreLocations(await _salesAnalyser.GetStoreLocations());
                await _cache.SetStoresAndTerminals(await _salesAnalyser.GetStoresAndTerminals());
                await _cache.SetTotalSalesRevenue(await _salesAnalyser.GetTotalSalesRevenue());
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SalesBackgroundService is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _timer?.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
    }
}
