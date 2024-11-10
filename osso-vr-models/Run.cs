namespace osso_vr_models;

public class Run
{
    public Guid Id { get; set; }
    public List<UserInteraction> Interactions { get; set; }

    public MetaData MetaData { get; set; }
}

public class MetaData()
{
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
}