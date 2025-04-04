using System;

public class CustomerId : Identity<Guid>
{
    public CustomerId(Guid value) : base(value) { }
}