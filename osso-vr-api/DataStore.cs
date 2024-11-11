using osso_vr_api.Controllers;
using osso_vr_models;
using System.Text.Json;

namespace osso_vr_api;

public class DataStore : IDataStore
{
    private Result _result;

    public IList<RunResult> Runs => _result.Runs;

    public DataStore(string resultsPath)
    {
        var resultJson = File.ReadAllText("c:\\data\\result.json");

        _result = JsonSerializer.Deserialize<Result>(resultJson);
    }

    private static string Ascending = "asc";
    private static string Descending = "desc";

    public Insights GetInsights(int count, string order)
    {
        var insights = new Insights
        {
            RunsCount = _result.TotalRuns,
            CompletedRunsPercentage = _result.CompletedRunsPercentage,
            PassedRunsPercentage = _result.PassedRunsPercentage,
            MedianTimeCompletedRuns = _result.MedianCompletedRunTime,
            TopRuns = string.Equals(order, Ascending, StringComparison.InvariantCultureIgnoreCase) ? _result.Runs.OrderBy(r => r.Duration).Take(count).ToList() : _result.Runs.OrderByDescending(r => r.Duration).Take(count).ToList()
        };

        return insights;
    }
}
public class Insights
{
    public int RunsCount { get; set; }
    public decimal CompletedRunsPercentage { get; set; }
    public decimal PassedRunsPercentage { get; set; }
    public double MedianTimeCompletedRuns { get; set; }
    public List<RunResult> TopRuns { get; set; }
}

public interface IDataStore
{
    public Insights GetInsights(int count, string order);
    public IList<RunResult> Runs { get; }
}
