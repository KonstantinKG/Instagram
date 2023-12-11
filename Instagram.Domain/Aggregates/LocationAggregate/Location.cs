using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.LocationAggregate;

public sealed class Location : AggregateRoot<long>
{
    public string Name { get; private set; }


    private Location(
        string name
        )
    {
        Name = name;
    }

    public static Location Create(
        string name
        )
    {
        return new Location(
            name
        );
    }
    
    # pragma warning disable CS8618
    private Location()
    {
    }
    # pragma warning disable CS8618
}