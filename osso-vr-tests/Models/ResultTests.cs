using FluentAssertions;
using osso_vr_models;
using System.Numerics;
using System.Reflection.Emit;

namespace osso_vr_tests.Models;
public class ResultTests
{
    [Test]
    public void WhenCreateResult_ThenSetRuns_ToResultRuns()
    {
        var runResultCount = RandomGenerator.NextInt(0, 100);

        var runs = Enumerable.Range(0, runResultCount).Select(_ => CreateRunResult(RandomGenerator.NextInt())).ToList();

        var result = Result.CreateResult(runs);

        result.Runs.Should().BeEquivalentTo(runs);
    }

    [Test]
    public void WhenCreateResult_ThenSetTotalRuns_ToCountOfruns()
    {
        var runResultCount = RandomGenerator.NextInt(0, 100);

        var runs = Enumerable.Range(0, runResultCount).Select(_ => CreateRunResult(RandomGenerator.NextInt())).ToList();

        var result = Result.CreateResult(runs);

        result.TotalRuns.Should().Be(runResultCount);
    }

    [Test]
    public void WhenCreateResult_ThenCalculateCompletedRuns()
    {
        var completedRunCount = RandomGenerator.NextInt(1, 100);
        var nonCompletedRunCount = RandomGenerator.NextInt(1, 100);

        var completedRuns = Enumerable.Range(0, completedRunCount).Select(_ => CreateRunResult(RandomGenerator.NextInt(), true));
        var noncompletedRuns = Enumerable.Range(0, nonCompletedRunCount).Select(_ => CreateRunResult(RandomGenerator.NextInt(), false));

        var expected = completedRunCount / (decimal)(completedRunCount + nonCompletedRunCount);

        var result = Result.CreateResult(completedRuns.Concat(noncompletedRuns).ToList());

        result.CompletedRunsPercentage.Should().Be(expected);
    }

    [Test]
    public void WhenCreateResult_ThenCalculatePassedRuns()
    {
        var passedRunsCount = RandomGenerator.NextInt(1, 100);
        var nonPassedRunsCount = RandomGenerator.NextInt(1, 100);

        var passedRuns = Enumerable.Range(0, passedRunsCount).Select(_ => CreateRunResult(RandomGenerator.NextInt(), true, true));
        var nonPassedRuns = Enumerable.Range(0, nonPassedRunsCount).Select(_ => CreateRunResult(RandomGenerator.NextInt(), true, false));

        var expected = passedRunsCount / (decimal)(passedRunsCount + nonPassedRunsCount);

        var result = Result.CreateResult(passedRuns.Concat(nonPassedRuns).ToList());

        result.PassedRunsPercentage.Should().Be(expected);
    }

    [Test]
    public void WhenCreateResult_ThenCalculateMedianCompletedTime()
    {
        var runsCount = RandomGenerator.NextInt(1, 100);

        var runs = Enumerable.Range(0, runsCount).Select(_ => CreateRunResult(RandomGenerator.NextInt())).ToList();

        var expected = runs.Select(r => r.Duration).Order().ElementAt(runsCount / 2);

        var result = Result.CreateResult(runs);

        result.MedianCompletedRunTime.Should().Be(expected);
    }

    public static RunResult CreateRunResult(int duration, bool completed = true, bool passed = true)
    {
        var runResult = new RunResult();

        runResult.UserName = Guid.NewGuid().ToString();
        runResult.Completed = completed;
        runResult.Passed = passed;
        runResult.Duration = duration;

        return runResult;
    }
}
