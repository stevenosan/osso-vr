namespace osso_vr_models;

public class Result
{
    public int TotalRuns { get; set; }
    public decimal CompletedRunsPercentage { get; set; }
    public decimal PassedRunsPercentage { get; set; }
    public double MedianCompletedRunTime { get; set; }
    public IList<RunResult> Runs { get; set; }

    public static Result CreateResult(IList<RunResult> runs)
    {
        var result = new Result();
        result.Runs = runs;
        result.TotalRuns = runs.Count;
        result.CompletedRunsPercentage = runs.Count(r => r.Completed) / (decimal)runs.Count;
        result.PassedRunsPercentage = runs.Count(r => r.Passed) / (decimal)runs.Count;
        result.MedianCompletedRunTime = CalculateMedianValue(runs.Select(r => r.Duration).Order().ToList());

        return result;
    }

    private static int CalculateMedianValue(IList<int> durations)
    {
        return durations.ElementAt(durations.Count / 2);
    }
}
