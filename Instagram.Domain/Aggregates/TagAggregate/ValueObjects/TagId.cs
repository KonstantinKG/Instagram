using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.TagAggregate.ValueObjects;

public sealed class TagId : ValueObject
{
    public Guid Value { get; private set; }
    
    private TagId(Guid value)
    {
        Value = value;
    }

    public static TagId Create() => new(Guid.NewGuid());
    public static TagId Fill(Guid guid) => new(guid);

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
    
# pragma warning disable CS8618
    private TagId()
    {
    }
# pragma warning disable CS8618
}