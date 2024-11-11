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
    private readonly IDataStore _dataStore;

    public RunsController(ILogger<RunsController> logger, IDataStore dataStore)
    {
        _logger = logger;

        _dataStore = dataStore;

        var resultJson = System.IO.File.ReadAllText("c:\\data\\result.json");

        _result = JsonSerializer.Deserialize<Result>(resultJson);
    }

    [Route("insights")]
    [HttpGet]
    public Insights GetInsights(int count, string order)
    {
        return _dataStore.GetInsights(count, order);
    }

    [HttpGet]
    public IList<RunResult> Get()
    {
        var runs = _dataStore.Runs;

        return runs;
    }

    [HttpGet("{runId}")]
    public ActionResult<RunResult> Get(Guid runId)
    {
        var run = _dataStore.Runs.SingleOrDefault(r => r.Id == runId);

        if(run is null)
        {
            return NotFound();
        }

        return run;
    }
}