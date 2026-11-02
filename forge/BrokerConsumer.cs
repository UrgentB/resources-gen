using Microsoft.Extensions.Hosting;
using Confluent.Kafka;

public class BrokerConsumer(ILogger<BrokerConsumer> logger, ConsumerConfig config) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        using var consumer = new ConsumerBuilder<string, int>(config).Build();
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

        return Task.CompletedTask;
    }
}