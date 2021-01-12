using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TempAnalyzer
{
    internal class MessageConsumer: BackgroundService
    {
        private readonly ILogger<MessageConsumer> _log;
        private readonly IConsumer<string, int> _consumer;

        public MessageConsumer(ILogger<MessageConsumer> log, IConsumer<string, int> consumer)
        {
            _log = log;
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            
            var i = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                
                _log.LogInformation(consumeResult.Message.Key + " - " + consumeResult.Message.Value);
                
                if (i++ % 1000 == 0)
                {
                    _consumer.Commit();
                }
            }
        }

        public override void Dispose()
        {
            _consumer.Dispose();
            base.Dispose();
        }
    }
}