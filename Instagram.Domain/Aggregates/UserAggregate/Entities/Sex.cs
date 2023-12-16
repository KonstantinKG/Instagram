using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.Entities;

public class Sex : Entity<SexId>
{
    public string Name { get; private set; }

    private Sex(
        SexId id,
        string name
        )
    : base(id)
    {
        Name = name;
    }

    public static Sex Create(
        string name
        )
    {
        return new Sex(
            SexId.Create(),
            name
        );
    }
    
    public static Sex Fill(
        Guid id,
        string name
    )
    {
        return new Sex(
            SexId.Fill(id),
            name
        );
    }
    
# pragma warning disable CS8618
    private Sex()
    {
    }
# pragma warning disable CS8618
}