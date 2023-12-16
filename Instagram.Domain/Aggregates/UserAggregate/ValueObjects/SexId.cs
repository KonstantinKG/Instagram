using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.ValueObjects;

public sealed class SexId : ValueObject
{
    public Guid Value { get; private set; }
    
    private SexId(Guid value)
    {
        Value = value;
    }

    public static SexId Create() => new(Guid.NewGuid());
    public static SexId Fill(Guid guid) => new(guid);

    public override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
    
# pragma warning disable CS8618
    private SexId()
    {
    }
# pragma warning disable CS8618
}