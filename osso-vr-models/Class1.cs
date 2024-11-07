namespace osso_vr_models;

public class Interaction
{
    public string Id { get; set; }
    public int GoalTime { get; set; }
}

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class Run
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
    public List<UserInteraction> Interactions { get; set; }


}

public class UserInteraction
{
    public string InteractionId { get; set; }
    public int StartTime { get; set; }
    public int EndTime { get; set; }
    public Interaction InteractionDefinition { get; set; }
}

public class Result
{
    public int TotalRuns { get; }
    public decimal CompletedRunsPercentage { get; }
    public decimal PassedRunsPercentage { get; }
    public double MedianCompletedRunTime { get; }
    public IList<RunResult> Runs { get; }

    public Result(IList<RunResult> runs)
    {
        TotalRuns = runs.Count;
        CompletedRunsPercentage = runs.Count(r => r.Completed) / runs.Count;
        PassedRunsPercentage = runs.Count(r => r.Passed) / runs.Count;
        MedianCompletedRunTime = runs.Select(r => r.Duration).Average();
    }
}

public class RunResult
{
    public Guid Id { get; }
    public DateTime Date { get; }
    public bool Completed { get; }
    public bool Passed { get; }


    public int Duration { get; }

    public List<InteractionResult> Interactions { get; }

    public RunResult(Run run, IList<InteractionResult> interactionResults, IEnumerable<Interaction> interactionDefinitions)
    {
        Interactions = [.. interactionResults];
        Duration = interactionResults.Select(i => i.ActualTime).Sum();
        Completed = interactionResults.Select(i => i.Id).SequenceEqual(interactionDefinitions.Select(i => i.Id)) && interactionResults.Any(i => !i.Completed);
        Passed = Completed && interactionResults.Any(i => !i.Passed);
        Duration = interactionResults.Sum(i => i.ActualTime);
        Id = run.Id;
        Date = run.Date;
    }
}

public class InteractionResult
{
    public string Id { get; set; }
    public int GoalTime { get; set; }
    public int ActualTime { get; set; }
    public bool Completed { get; set; }
    public bool Passed { get; set; }

    public InteractionResult(UserInteraction userInteraction)
    {
        Id = userInteraction.InteractionId;
        GoalTime = userInteraction.InteractionDefinition.GoalTime;
        ActualTime = userInteraction.EndTime != 0 ? userInteraction.EndTime - userInteraction.StartTime : 0;
        Completed = userInteraction.EndTime > 0;
        Passed = ActualTime < GoalTime;
    }
}