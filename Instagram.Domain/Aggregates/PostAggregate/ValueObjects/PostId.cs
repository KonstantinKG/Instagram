using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.ValueObjects;

public sealed class PostId : ValueObject
{
    public Guid Value { get; }
    
    private PostId(Guid value)
    {
        Value = value;
    }

    public static PostId Create() => new(Guid.NewGuid());
    public static PostId Fill(Guid guid) => new(guid);

    public override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
    
# pragma warning disable CS8618
    private PostId()
    {
    }
# pragma warning disable CS8618
}