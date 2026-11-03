using System;
using Confluent.Kafka;

public class BrokerProducer(ProducerConfig config, ILogger<BrokerProducer> logger, GenModel model) : BackgroundService
{
    IProducer<string, int> producer = new ProducerBuilder<string, int>(config).Build();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = model.Tick();
            if (result.HasValue)
            {
                var (resource, count) = result.Value;
                logger.LogInformation($"Producing message '{count}' for resource '{resource}'");
                producer.Produce("test-topic", new Message<string, int> { Key = resource, Value = count });
            }
            await Task.Delay(2000, stoppingToken);
        }
    }
}
