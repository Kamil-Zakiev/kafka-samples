using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WeatherLoader
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var producerConfig = context.Configuration.GetSection("ProducerConfig").Get<ProducerConfig>();
                    var producer = new ProducerBuilder<string, int>(producerConfig).Build();
                    
                    services.AddSingleton<IWeatherProvider, MockWeatherProvider>();
                    services.AddSingleton<IWeatherPublisher>(_ => new WeatherKafkaPublisher(producer, "WeatherTopic"));
                    services.AddHostedService<WeatherPullService>();
                });
        }
    }
}