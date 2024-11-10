using FluentAssertions;
using osso_vr_models;

namespace osso_vr_tests.Models;
public partial class InteractionResultsTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CreateInteractionResult_SetsId()
    {
        var userInteraction = CreateUserInteraction(RandomGenerator.NextInt(1, 10000), RandomGenerator.NextInt(1, 10000), RandomGenerator.NextInt());
        
        var interactionResult = InteractionResult.CreateInteractionResult(userInteraction);

        interactionResult.Id.Should().Be(userInteraction.InteractionId);
    }

    [Test]
    public void CreateInteractionResult_SetsGoalTime()
    {
        var userInteraction = CreateUserInteraction(RandomGenerator.NextInt(1, 10000), RandomGenerator.NextInt(1, 10000), RandomGenerator.NextInt());

        var interactionResult = InteractionResult.CreateInteractionResult(userInteraction);

        interactionResult.GoalTime.Should().Be(userInteraction.InteractionDefinition.GoalTime);
    }

    [Test]
    public void CreateInteractionResult_WhenEndTimeZero_SetCompletedToFalse()
    {
        var userInteraction = CreateUserInteraction(RandomGenerator.NextInt(1, 10000), 0, RandomGenerator.NextInt());

        var interactionResult = InteractionResult.CreateInteractionResult(userInteraction);

        interactionResult.Completed.Should().Be(false);
    }

    [Test]
    public void CreateInteractionResult_WhenNotCompleted_SetActualTimeToZero()
    {
        var userInteraction = CreateUserInteraction(RandomGenerator.NextInt(1, 10000), 0, RandomGenerator.NextInt());

        var interactionResult = InteractionResult.CreateInteractionResult(userInteraction);

        interactionResult.ActualTime.Should().Be(0);
    }

    [Test]
    public void CreateInteractionResult_WhenEndTimeAndStartTimeSet_ThenSetActualTimeToDifference()
    {
        var startTime = RandomGenerator.NextInt(1, 500);
        var endTime = RandomGenerator.NextInt(startTime, 1000);

        var userInteraction = CreateUserInteraction(startTime, endTime, RandomGenerator.NextInt());

        var interactionResult = InteractionResult.CreateInteractionResult(userInteraction);

        interactionResult.ActualTime.Should().Be(endTime - startTime);
    }

    [Test]
    public void CreateInteractionResult_WhenEndTimeAndStartTimeSet_ThenCompletedIsTrue()
    {
        var startTime = RandomGenerator.NextInt(1, 500);
        var endTime = RandomGenerator.NextInt(startTime, 1000);

        var userInteraction = CreateUserInteraction(startTime, endTime, RandomGenerator.NextInt());

        var interactionResult = InteractionResult.CreateInteractionResult(userInteraction);

        interactionResult.Completed.Should().Be(true);
    }

    [Test]
    public void CreateInteractionResult_WhenActualTimeLessThanGoalTime_ThenPassedIsTrue()
    {
        var startTime = RandomGenerator.NextInt(1, 500);
        var endTime = RandomGenerator.NextInt(startTime, 1000);

        var actualTime = endTime - startTime;
        var goalTime = RandomGenerator.NextInt(actualTime, 10000);

        var userInteraction = CreateUserInteraction(startTime, endTime, goalTime);

        var interactionResult = InteractionResult.CreateInteractionResult(userInteraction);

        interactionResult.Completed.Should().Be(true);
    }

    [Test]
    public void CreateInteractionResult_WhenActualTimeGreaterThanGoalTime_ThenPassedIsFalse()
    {
        var startTime = RandomGenerator.NextInt(1, 500);
        var endTime = RandomGenerator.NextInt(startTime, 1000);

        var actualTime = endTime - startTime;
        var goalTime = RandomGenerator.NextInt(0, actualTime);

        var userInteraction = CreateUserInteraction(startTime, endTime, goalTime);

        var interactionResult = InteractionResult.CreateInteractionResult(userInteraction);

        interactionResult.Completed.Should().Be(true);
    }

    public static UserInteraction CreateUserInteraction(int startTime, int endTime, int goalTime)
    {
        var interactionDefinition = new Interaction
        {
            Id = Guid.NewGuid().ToString(),
            GoalTime = goalTime
        };

        var userInteraction = new UserInteraction
        {
            InteractionId = interactionDefinition.Id,
            InteractionDefinition = interactionDefinition,
            EndTime = endTime,
            StartTime = startTime
        };

        return userInteraction;
    }

    public static UserInteraction CreateUserInteraction(string interactionId, bool completed = true, bool passed = true)
    {
        var startTime = RandomGenerator.NextInt(1, 100);
        var endTime = completed ? RandomGenerator.NextInt(startTime, 1000) : 0;
        var goalTime = passed ? RandomGenerator.NextInt(startTime-endTime, int.MaxValue) : RandomGenerator.NextInt();

        var interactionDefinition = new Interaction
        {
            Id = interactionId,
            GoalTime = goalTime
        };

        var userInteraction = new UserInteraction
        {
            InteractionId = interactionDefinition.Id,
            InteractionDefinition = interactionDefinition,
            EndTime = endTime,
            StartTime = startTime
        };

        return userInteraction;
    }
}
