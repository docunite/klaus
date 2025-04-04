namespace EventSourcingTest.Domain;

public class CreditsBalance : IntValueObject
{
    private readonly LimitThreshold limitThreshold;

    public CreditsBalance(int value, LimitThreshold limitThreshold) : base(value)
    {
        this.limitThreshold = limitThreshold;
    }

    public LimitThreshold LimitThreshold => limitThreshold;

    public CreditsBalance Add(Credits credits) => new CreditsBalance(Value + credits.Value, limitThreshold);
    public CreditsBalance Subtract(Credits credits) => new CreditsBalance(Value - credits.Value, limitThreshold);
}