using EventSourcingTest.Infrastructure;

namespace EventSourcingTest.Domain;

public class CreditsBalance(int value, LimitThreshold limitThreshold) : IntValueObject(value)
{
    public bool IsTheSameBalance(CreditsBalance balance)
    {
        return Equals(balance.LimitThreshold, LimitThreshold) && balance.Value == Value;
    }
    public LimitThreshold LimitThreshold => limitThreshold;

    public CreditsBalance Add(Credits credits) => new CreditsBalance(Value + credits.Value, limitThreshold);
    public CreditsBalance Subtract(Credits credits) => new CreditsBalance(Value - credits.Value, limitThreshold);
}