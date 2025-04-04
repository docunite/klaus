using System;

namespace EventSourcingTest.Snapshots;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class PersistedAttribute : Attribute
{
}