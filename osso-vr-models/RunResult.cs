namespace osso_vr_models;

public class RunResult
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public bool Completed { get; set; }
    public bool Passed { get; set; }
    public int Duration { get; set; }
    public string UserName { get; set; }
    public List<InteractionResult> Interactions { get; set; }

    public static RunResult CreateRunResult(Run run, IEnumerable<string> interactionIds, IEnumerable<User> users)
    {
        var interactionResults = run.Interactions.Select(i => InteractionResult.CreateInteractionResult(i)).ToList();

        var runResult = new RunResult();

        runResult.Interactions = interactionResults;
        runResult.Duration = interactionResults.Select(i => i.ActualTime).Sum();
        runResult.Completed = interactionResults.Select(i => i.Id).SequenceEqual(interactionIds) && !interactionResults.Any(i => i.Completed == false);
        runResult.Passed = runResult.Completed && interactionResults.Any(i => !i.Passed);
        runResult.Duration = interactionResults.Sum(i => i.ActualTime);
        runResult.Id = run.Id;
        runResult.Date = run.MetaData.Date;
        runResult.UserName = users.Single(u => u.Id == run.MetaData.UserId).Name;

        return runResult;
    }
}