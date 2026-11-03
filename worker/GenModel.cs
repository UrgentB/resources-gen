using Microsoft.Extensions.Logging;

public enum ModelState
{
    Init,
    Running,
    Done
}

public class GenModelConfig
{
    public List<GenCase> InitCases { get; set; } = new();
    public List<GenCase> ExpeditionCases { get; set; } = new();
    public List<LootCase> LootCases { get; set; } = new();
}

public record GenCase(int Chance, string Status, ModelState Conclusion);
public record LootCase(int Chance, string Loot);

public class GenModel(ILogger<GenModel> logger, GenModelConfig config)
{
    private record GenCase(int Chance, string Status, ModelState Conclusion);
    private record LootCase(int Chance, string Loot);

    private ModelState state = ModelState.Init;

    public (string resource, int count)? Tick()
    {
        switch (state)
        {
            case ModelState.Init:
                foreach(var c in config.InitCases)
                {
                    if (new Random().Next(0, 100) < c.Chance || config.InitCases.Last() == c)
                    {
                        logger.LogInformation($"Init case: {c.Status} with chance {c.Chance}");
                        state = c.Conclusion;
                        break;
                    }
                }
                break;
            case ModelState.Running:
                foreach(var c in config.ExpeditionCases)
                {
                    if (new Random().Next(0, 100) < c.Chance || config.ExpeditionCases.Last() == c)
                    {
                        logger.LogInformation($"Expedition case: {c.Status} with chance {c.Chance}");
                        state = c.Conclusion;
                        break;
                    }
                }
                break;
            case ModelState.Done:
                foreach(var c in config.LootCases)
                {
                    if (new Random().Next(0, 100) < c.Chance || config.LootCases.Last() == c)
                    {
                        logger.LogInformation($"Loot case: {c.Loot} with chance {c.Chance}");
                        return (c.Loot, 1);
                    }
                }
                break;
        }
        return null;
    }
}

