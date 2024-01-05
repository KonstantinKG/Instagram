using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.TagAggregate;

public sealed class Tag : AggregateRoot<Guid>
{
    public required string Name { get; set; }
    
}