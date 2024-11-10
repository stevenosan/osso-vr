namespace osso_vr_models;

public class InteractionResult
{
    public string Id { get; set; }
    public int GoalTime { get; set; }
    public int ActualTime { get; set; }
    public bool Completed { get; set; }
    public bool Passed { get; set; }

    public static InteractionResult CreateInteractionResult(UserInteraction userInteraction)
    {
        var interactionResult = new InteractionResult();
        interactionResult.Id = userInteraction.InteractionId;
        interactionResult.GoalTime = userInteraction.InteractionDefinition.GoalTime;
        interactionResult.Completed = userInteraction.EndTime > 0;
        interactionResult.ActualTime = interactionResult.Completed ? userInteraction.EndTime - userInteraction.StartTime : 0;
        interactionResult.Passed = interactionResult.Completed && interactionResult.ActualTime <= interactionResult.GoalTime;

        return interactionResult;
    }
}