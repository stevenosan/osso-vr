using Microsoft.AspNetCore.Mvc;
using osso_vr_models;
using System.Text.Json;

namespace osso_vr_api.Controllers;
[ApiController]
[Route("[controller]")]
public class RunsController : ControllerBase
{
    private readonly ILogger<RunsController> _logger;

    private readonly Result _result;

    public RunsController(ILogger<RunsController> logger)
    {
        _logger = logger;

        var resultJson = System.IO.File.ReadAllText("c:\\data\\result.json");

        _result = JsonSerializer.Deserialize<Result>(resultJson);
    }

    private static string Ascending = "asc";
    private static string Descending = "desc";

    [Route("insights")]
    [HttpGet]
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

    [HttpGet]
    public IList<RunResult> Get()
    {
        var runs = _result.Runs;

        return runs;
    }

    [HttpGet("{runId}")]
    public ActionResult<RunResult> Get(Guid runId)
    {
        var run = _result.Runs.SingleOrDefault(r => r.Id == runId);

        if(run is null)
        {
            return NotFound();
        }

        return run;
    }

    public class Insights
    {
        public int RunsCount { get; set; }
        public decimal CompletedRunsPercentage { get; set; }
        public decimal PassedRunsPercentage { get; set; }
        public double MedianTimeCompletedRuns { get; set; }
        public List<RunResult> TopRuns { get; set; }
    }
}