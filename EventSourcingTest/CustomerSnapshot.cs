using System;

public class CustomerSnapshot
{
    public CustomerId Id { get; set; } = default!;
    public CustomerName Name { get; set; } = default!;
}

public class CustomerId : Identity<Guid>
{
    public CustomerId(Guid value) : base(value) { }
}

public class CustomerName : ValueObject<string>
{
    public CustomerName(string value) : base(value) { }
}