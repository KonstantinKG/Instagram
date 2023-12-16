using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.ValueObjects;

public sealed class UserProfileId : ValueObject
{
    public Guid Value { get; private set; }
    
    private UserProfileId(Guid value)
    {
        Value = value;
    }

    public static UserProfileId Create() => new(Guid.NewGuid());
    public static UserProfileId Fill(Guid guid) => new(guid);

    public override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
    
# pragma warning disable CS8618
    private UserProfileId()
    {
    }
# pragma warning disable CS8618
}