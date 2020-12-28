using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;

namespace WeatherLoader
{
    internal class MockWeatherProvider : IWeatherProvider
    {
        private static readonly Random _random = new();
        
        public Task<IReadOnlyCollection<Weather>> GetCurrent(CancellationToken cancellationToken)
        {
            var itemsCount = _random.Next(100, 500);
            var faker = new Faker<Weather>()
                .RuleFor(x => x.City, x => x.Address.City())
                .RuleFor(x => x.Temperature, x => x.Random.Int(-32, 32));

            var weathers = faker.Generate(itemsCount);
            return Task.FromResult<IReadOnlyCollection<Weather>>(weathers);
        }
    }
}