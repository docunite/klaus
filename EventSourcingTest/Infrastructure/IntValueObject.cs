namespace EventSourcingTest.Infrastructure;

public class IntValueObject(int value) : ValueObject<int>(value);
public class StringValueObject(string value) : ValueObject<string>(value);