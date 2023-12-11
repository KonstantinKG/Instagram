using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.TagAggregate;

public sealed class Tag : AggregateRoot<long>
{
    public string Name { get; private set; }


    private Tag(
        string name
        )
    {
        Name = name;
    }

    public static Tag Create(
        string name
        )
    {
        return new Tag(
            name
        );
    }
    
    # pragma warning disable CS8618
    private Tag()
    {
    }
    # pragma warning disable CS8618
}