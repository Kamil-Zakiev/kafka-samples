using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WeatherLoader
{
    internal class WeatherPullService : BackgroundService
    {
        private readonly IWeatherProvider _provider;
        private readonly IWeatherPublisher _publisher;
        private readonly ILogger<WeatherPullService> _log;

        public WeatherPullService(IWeatherProvider provider, IWeatherPublisher publisher, ILogger<WeatherPullService> log)
        {
            _provider = provider;
            _publisher = publisher;
            _log = log;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var weather = await _provider.GetCurrent(stoppingToken);
                await _publisher.Publish(weather, stoppingToken);
                _log.LogInformation("Published {weatherCount} weather items", weather.Count);
            }
        }
    }
}