using System;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kafka.SDK
{
    public static class KafkaProducerExtensions
    {
        public static IServiceCollection AddKafkaProducer(this IServiceCollection services,
            IConfiguration configuration)
        {
            var producerConfig = configuration.GetSection("ProducerConfig").Get<ProducerConfig>();
            services.AddSingleton(_ => new ProducerBuilder<string, int>(producerConfig).Build());
            return services;
        }
        
        public static IServiceCollection AddKafkaProducer(this IServiceCollection services,
            ProducerConfig producerConfig)
        {
            services.AddSingleton(_ => new ProducerBuilder<string, int>(producerConfig).Build());
            return services;
        }
    }
}