namespace EventSourcingTest.Domain;

public class IntValueObject(int value) : ValueObject<int>(value);