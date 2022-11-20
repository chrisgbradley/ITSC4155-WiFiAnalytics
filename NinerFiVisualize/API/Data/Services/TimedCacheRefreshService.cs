using System.Threading;

namespace NinerFiVisualize.Data.Services
{
    public class TimedCacheRefreshService : BackgroundService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedCacheRefreshService> _logger;
        private Timer? _timer = null;
        private readonly TimeSpan QUERY_INTERVAL = TimeSpan.FromMinutes(30);

        public TimedCacheRefreshService(ILogger<TimedCacheRefreshService> logger)
        {
            _logger = logger;
        }

        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Cache Refresh Service is working. ", DateTimeOffset.Now);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Cache Refresh Service running.");

            while (!cancellationToken.IsCancellationRequested)
            {
                DoWork(null);
                await Task.Delay(QUERY_INTERVAL.Milliseconds);
            }
        }
    }
}