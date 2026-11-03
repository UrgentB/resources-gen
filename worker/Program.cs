using System.Text.Json;
using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

var modelConfig = JsonSerializer.Deserialize<GenModelConfig>(File.ReadAllText("model.json"));

var brokerConfig = new ProducerConfig
{
    BootstrapServers = builder.Configuration["Kafka:BootstrapServers"]
};

builder.Services.AddSingleton(modelConfig);
builder.Services.AddSingleton<GenModel>();
builder.Services.AddSingleton(brokerConfig);
builder.Services.AddHostedService<BrokerProducer>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
