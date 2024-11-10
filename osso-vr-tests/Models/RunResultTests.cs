using FluentAssertions;
using osso_vr_models;

namespace osso_vr_tests.Models;
public class RunResultTests
{
    [Test]
    public void WhenCreateRunResult_ThenSetInteractionsToRunsInteractions()
    {

        var interactionIds = CreateInteractionIds(20);
        var users = CreateUsers(5).ToList();

        var run = CreateRun(users, interactionIds);

        var runResult = RunResult.CreateRunResult(run, interactionIds, users);

        runResult.Interactions.Count.Should().Be(run.Interactions.Count);
    }

    [Test]
    public void WhenCreateRunResult_ThenSetDurationAsSumOfInteractionActualTimes()
    {
        var interactionIds = CreateInteractionIds(20);
        var users = CreateUsers(5).ToList();

        var run = CreateRun(users, interactionIds);

        var runResult = RunResult.CreateRunResult(run, interactionIds, users);

        var expected = runResult.Interactions.Select(i => i.ActualTime).Sum();

        runResult.Duration.Should().Be(expected);
    }

    [Test]
    public void WhenCreateRunResult_CompletedSetToTrue_WhenAllInteractionsPresent_AndAllAreCompleted()
    {
        var interactionIds = CreateInteractionIds(20);
        var users = CreateUsers(5).ToList();

        var run = CreateRun(users, interactionIds, true);

        var runResult = RunResult.CreateRunResult(run, interactionIds, users);

        runResult.Completed.Should().BeTrue();
    }

    [Test]
    public void WhenCreateRunResult_CompletedSetToFalse_WhenNotAllInteractionsPresent()
    {
        var interactionIds = CreateInteractionIds(20);
        var users = CreateUsers(5).ToList();

        var run = CreateRun(users, interactionIds, false);

        var runResult = RunResult.CreateRunResult(run, interactionIds, users);

        runResult.Completed.Should().BeFalse();
    }

    [Test]
    public void WhenCreateRunResult_CompletedSetToFalse_WhenAllInteractionsPresent_ButOneNotCompleted()
    {
        var interactionIds = CreateInteractionIds(20);
        var users = CreateUsers(5).ToList();

        var run = CreateRun(users, interactionIds, true, false);

        var runResult = RunResult.CreateRunResult(run, interactionIds, users);

        runResult.Completed.Should().BeFalse();
    }

    [Test]
    public void WhenCreateRunResult_PassedSetToTrue_WhenAllInteractionsPresent_Completed_AndPassed()
    {
        var interactionIds = CreateInteractionIds(20);
        var users = CreateUsers(5).ToList();

        var run = CreateRun(users, interactionIds);

        var runResult = RunResult.CreateRunResult(run, interactionIds, users);

        runResult.Passed.Should().BeTrue();
    }

    [Test]
    public void WhenCreateRunResult_PassedSetToFalse_WhenNotCompleted()
    {
        var interactionIds = CreateInteractionIds(20);
        var users = CreateUsers(5).ToList();

        var run = CreateRun(users, interactionIds, true, false);

        var runResult = RunResult.CreateRunResult(run, interactionIds, users);

        runResult.Passed.Should().BeFalse();
    }

    public static Run CreateRun(IEnumerable<User> users, IEnumerable<string> interactionIds)
    {
        var interactions = Enumerable.Range(0, interactionIds.Count()).Select(i => InteractionResultsTests.CreateUserInteraction(interactionIds.ElementAt(i), true));

        var user = users.First();

        var run = new Run { MetaData = new MetaData { UserId = user.Id }, Interactions = interactions.ToList() };

        return run;
    }

    public static Run CreateRun(IEnumerable<User> users, IEnumerable<string> interactionIds, bool allPresent = true, bool allCompleted = true)
    {
        var range = allPresent ? interactionIds.Count() : interactionIds.Count() - RandomGenerator.NextInt(1, interactionIds.Count());

        var interactions = Enumerable.Range(0, range).Select(i => InteractionResultsTests.CreateUserInteraction(interactionIds.ElementAt(i), allCompleted));

        var user = users.First();

        var run = new Run { MetaData = new MetaData { UserId = user.Id }, Interactions = interactions.ToList() };

        return run;
    }

    public static IEnumerable<string> CreateInteractionIds(int count)
    {
        return Enumerable.Range(1, count).Select(i => $"Interaction{i}");
    }

    public static IEnumerable<User> CreateUsers(int count)
    {
        return Enumerable.Range(1, count).Select(_ => new User { Id = Guid.NewGuid(), Name = Guid.NewGuid().ToString() });
    }
}

