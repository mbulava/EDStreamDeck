using BarRaider.SdTools;
using EliteAPI.Abstractions;
using EliteAPI.Abstractions.Events;
using EliteAPI.Events;

namespace EliteDangerous_Stream.Services
{
    public class EliteWatcherBackgroundService : BackgroundService
    {
        private static bool _stopRequested = false;
        private readonly ILogger _logger;   
        private readonly IEliteDangerousApi _api;
        
        public static void RequestStop()
        {
            _stopRequested = true;
        }

        public EliteWatcherBackgroundService(ILogger<EliteWatcherBackgroundService> logger,IEliteDangerousApi edWatacher) 
        { 
            _logger = logger;
            _api = edWatacher;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SDWrapper.Run(new string[0]);
            await _api.StartAsync();
            _api.Events.OnAny(EliteEventHandler);
            while(!_stopRequested)
            {
                await Task.Delay(1000);
            }
            
        }

        private void EliteEventHandler(IEvent e, EventContext context)
        {
            _logger.LogInformation($"Elite Dangerous Event: {e.Event}");
        }
    }
}
