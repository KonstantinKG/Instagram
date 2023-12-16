using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.ValueObjects;

public sealed class UserId : ValueObject
{
    public Guid Value { get; private set; }
    
    private UserId(Guid value)
    {
        Value = value;
    }

    public static UserId Create() => new(Guid.NewGuid());
    public static UserId Fill(Guid guid) => new(guid);

    public override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
    
# pragma warning disable CS8618
    private UserId()
    {
    }
# pragma warning disable CS8618
}