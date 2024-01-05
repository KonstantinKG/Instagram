using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.LocationAggregate;

public sealed class Location : AggregateRoot<long>
{
    public required string Name { get; set; }
}