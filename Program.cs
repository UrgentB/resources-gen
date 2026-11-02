using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "test-consumer-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};