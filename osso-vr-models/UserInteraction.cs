namespace osso_vr_models;

public class UserInteraction
{
    public string InteractionId { get; set; }
    public int StartTime { get; set; }
    public int EndTime { get; set; }
    public Interaction InteractionDefinition { get; set; }
}
