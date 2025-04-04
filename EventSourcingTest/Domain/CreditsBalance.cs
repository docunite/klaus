using System.Text.Json.Serialization;
using EventSourcingTest.Infrastructure;

namespace EventSourcingTest.Domain;

[method: JsonConstructor]
public class CreditsBalance(int value, LimitThreshold limit) : IntValueObject(value)
{
    public bool IsTheSameBalance(CreditsBalance balance)
    {
        return Equals(balance.TheLimit, TheLimit) && balance.Value == Value;
    }
    public LimitThreshold TheLimit => limit;

    public CreditsBalance Add(Credits credits) => new CreditsBalance(Value + credits.Value, TheLimit);
    public CreditsBalance Subtract(Credits credits) => new CreditsBalance(Value - credits.Value, TheLimit);
}