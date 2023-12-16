using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.LocationAggregate.ValueObjects;

public sealed class LocationId : ValueObject
{
    public Guid Value { get; }
    
    private LocationId(Guid value)
    {
        Value = value;
    }

    public static LocationId Create() => new(Guid.NewGuid());
    public static LocationId Fill(Guid guid) => new(guid);

    public override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
    
# pragma warning disable CS8618
    private LocationId()
    {
    }
# pragma warning disable CS8618
}