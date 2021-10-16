using Confluent.Kafka;
using Kafka.SDK;
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
                    services.AddSingleton<IWeatherProvider, MockWeatherProvider>();
                    services.AddKafkaProducer(context.Configuration);
                    services.AddSingleton<IWeatherPublisher>(sp =>
                    {
                        var producer = sp.GetRequiredService<IProducer<string, int>>();
                        return new WeatherKafkaPublisher(producer, "WeatherTopic");
                    });
                    services.AddHostedService<WeatherPullService>();
                });
        }
    }
}