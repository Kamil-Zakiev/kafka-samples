using System;
using System.Collections.Generic;
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
            foreach (var weather in weathers)
            {
                var message = new Message<string, int>()
                {
                    Key = weather.City,
                    Value = weather.Temperature
                };
                
                await _producer.ProduceAsync(_topic, message, cancellationToken);
            }
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}