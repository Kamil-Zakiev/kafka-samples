using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace WeatherLoader
{
    internal sealed class WeatherKafkaPublisher : IWeatherPublisher, IDisposable
    {
        private readonly IProducer<string, int> _producer;
        private readonly string _topic;

        public WeatherKafkaPublisher(IProducer<string, int> producer, string topic)
        {
            _producer = producer;
            _topic = topic;
        }
        
        public async Task Publish(IReadOnlyCollection<Weather> weathers, CancellationToken cancellationToken)
        {
            var tasks = weathers
                .Select(weather =>
                {
                    var message = new Message<string, int>()
                    {
                        Key = weather.City,
                        Value = weather.Temperature
                    };

                    return _producer.ProduceAsync(_topic, message, cancellationToken);
                })
                .ToArray();

            await Task.WhenAll(tasks);
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}