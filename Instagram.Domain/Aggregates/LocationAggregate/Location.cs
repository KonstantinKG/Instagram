using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.LocationAggregate;

public sealed class Location : AggregateRoot<long>
{
    public string Name { get; private set; }


    private Location(
        long id, 
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
            default,
            name
        );
    }
    
    public static Location Fill(
        long id,
        string name
    )
    {
        return new Location(
            id,
            name
        );
    }
    
    # pragma warning disable CS8618
    private Location()
    {
    }
    # pragma warning disable CS8618
}