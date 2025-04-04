using EventSourcingTest.Infrastructure;

namespace EventSourcingTest.Domain;

public class Credits(int value) : IntValueObject(value);