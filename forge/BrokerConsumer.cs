using Microsoft.Extensions.Hosting;
using Confluent.Kafka;

public class BrokerConsumer(ConsumerConfig config, ILogger<BrokerConsumer> logger) : BackgroundService
{
    private ILogger<BrokerConsumer> logger;
    private IConsumer<string, int> consumer = new ConsumerBuilder<string, int>(config).Build();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.Subscribe("test-topic");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);
                logger.LogInformation($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
            }
            catch (ConsumeException e)
            {
                logger.LogError($"Error occurred: {e.Error.Reason}");
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}