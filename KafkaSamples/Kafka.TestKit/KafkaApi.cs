using System.Threading.Tasks;
using Confluent.Kafka;

namespace Kafka.TestKit
{
    public static class KafkaApi
    {
        public static async Task<DeliveryResult<string, int>> ProduceAsync(string topic, int message)
        {
            
            var producer = new ProducerBuilder<string, int>(new ProducerConfig()
            {
                BootstrapServers = "localhost:9092, localhost:9093, localhost:9094"
            }).Build();

            return await producer.ProduceAsync(topic, new Message<string, int>()
            {
                Key = "some-key",
                Value = message
            });
        }

        public static async Task ConsumeAsync(string topic, long offset)
        {
            var consumer = new ConsumerBuilder<string, int>(new ConsumerConfig()
            {
                GroupId = "my-test-group",
                EnableAutoCommit = false,
                BootstrapServers = "localhost:9092, localhost:9093, localhost:9094",
                AutoOffsetReset = AutoOffsetReset.Earliest
            }).Build();
            consumer.Subscribe(topic);

            var tcs = new TaskCompletionSource();
            _ = Task.Run(() =>
            {
                while (true)
                {
                    var consumeResult = consumer.Consume();
                    if (consumeResult.Offset == offset)
                    {
                        tcs.SetResult();
                    }
                }
            });

            await tcs.Task;
        }
    }
    
}