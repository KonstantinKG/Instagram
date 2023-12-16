using Instagram.Domain.Aggregates.LocationAggregate.ValueObjects;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.LocationAggregate;

public sealed class Location : AggregateRoot<LocationId>
{
    public string Name { get; private set; }


    private Location(
        LocationId id,
        string name
        )
    : base(id)
    {
        Name = name;
    }

    public static Location Create(
        string name
        )
    {
        return new Location(
            LocationId.Create(),
            name
        );
    }
    
    public static Location Fill(
        Guid id,
        string name
    )
    {
        return new Location(
            LocationId.Fill(id),
            name
        );
    }
    
    # pragma warning disable CS8618
    private Location()
    {
    }
    # pragma warning disable CS8618
}