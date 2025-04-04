using System.Collections.Generic;

public abstract class Identity<T>
{
    public T Value { get; }

    protected Identity(T value)
    {
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Identity<T> other && EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public override string ToString() => Value?.ToString() ?? "";
}