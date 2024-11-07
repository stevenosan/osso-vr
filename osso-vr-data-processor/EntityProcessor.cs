using osso_vr_models;
using System.Text.Json;
using Interaction = osso_vr_models.Interaction;

namespace osso_vr_data_processor;
public static class EntityProcessor
{
    public static T ProcessFile<T>(string file)
    {
        var contents = File.ReadAllText(file);

        return JsonSerializer.Deserialize<T>(contents);
    }
}

public class FileReader
{
    public T ReadFile<T>(string file)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var fileContent = File.ReadAllText(file);
        return JsonSerializer.Deserialize<T>(fileContent, options);
    }
}

public class ResultGenerated
{
    public static Result GenerateResult(IList<User> users, IList<Interaction> interactions, IList<Run> runs)
    {
        var result = new Result();

        result.Runs = runs.Select(r =>
        new RunResult
        {
            Id = r.Id,
            Date = r.Date,
            Interactions = r.Interactions.Select(i =>
            {
                var interaction = interactions.Single(interaction => i.InteractionId == interaction.Id);
                return GenerateInteractionResult(interaction, i);
            }).ToList()
        }).ToList();
        return result;
    }

    public static InteractionResult GenerateInteractionResult(Interaction interaction, UserInteraction userInteraction)
    {
        var result = new InteractionResult();

        var actualTime = userInteraction.EndTime == 0 ? 0 : userInteraction.EndTime - userInteraction.StartTime;

        return new InteractionResult
        {
            Id = userInteraction.InteractionId,
            GoalTime = interaction.GoalTime,
            ActualTime = actualTime,
            Completed = userInteraction.EndTime > 0,
            Passed = actualTime < interaction.GoalTime
        };
    }
}

