using System;

namespace EventSourcingTest.Domain;

public class CustomerId : Identity<Guid>
{
    public CustomerId(Guid value) : base(value) { }
}