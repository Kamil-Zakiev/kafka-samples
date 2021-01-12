using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TempAnalyzer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var consumerConfig = context.Configuration.GetSection("ConsumerConfig").Get<ConsumerConfig>();
                    var consumer = new ConsumerBuilder<string, int>(consumerConfig).Build();
                    consumer.Subscribe("WeatherTopic");

                    services.AddHostedService(sp =>
                        new MessageConsumer(sp.GetRequiredService<ILogger<MessageConsumer>>(), consumer));
                })
                .Build()
                .Run();
        }
    }
}