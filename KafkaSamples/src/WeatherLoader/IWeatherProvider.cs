using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherLoader
{
    internal interface IWeatherProvider
    {
        Task<IReadOnlyCollection<Weather>> GetCurrent(CancellationToken cancellationToken);
    }
}