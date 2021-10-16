using System;
using System.Threading.Tasks;
using Xunit;
using Kafka.TestKit;

namespace WeatherLoader.IntegrationTests
{
    public class UnitTest1
    {
        
        
        [Fact]
        public async Task Test1()
        {
            var dr = await KafkaApi.ProduceAsync("test", 123);
            await KafkaApi.ConsumeAsync("test", dr.Offset);
        }
    }
}