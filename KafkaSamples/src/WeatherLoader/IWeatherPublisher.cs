using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherLoader
{
    internal interface IWeatherPublisher
    {
        Task Publish(IReadOnlyCollection<Weather> weather, CancellationToken cancellationToken);
    }
}