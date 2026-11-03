using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

var config = new ConsumerConfig
{
    BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
    GroupId = "test-consumer-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

builder.Services.AddSingleton(config);
builder.Services.AddHostedService<BrokerConsumer>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
