using Instagram.Domain.Aggregates.LocationAggregate.ValueObjects;
using Instagram.Domain.Aggregates.TagAggregate.ValueObjects;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.TagAggregate;

public sealed class Tag : AggregateRoot<TagId>
{
    public string Name { get; private set; }


    private Tag(
        TagId id,
        string name
        )
    : base(id)
    {
        Name = name;
    }

    public static Tag Create(
        string name
        )
    {
        return new Tag(
            TagId.Create(), 
            name
        );
    }

    public static Tag Fill(
        Guid id,
        string name
    )
    {
        return new Tag(
            TagId.Fill(id), 
            name
        );
    }
    
    # pragma warning disable CS8618
    private Tag()
    {
    }
    # pragma warning disable CS8618
}