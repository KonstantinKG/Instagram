using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.ValueObjects;

public sealed class PostGalleryId : ValueObject
{
    public Guid Value { get; }
    
    private PostGalleryId(Guid value)
    {
        Value = value;
    }

    public static PostGalleryId Create() => new(Guid.NewGuid());
    public static PostGalleryId Fill(Guid guid) => new(guid);

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
    
# pragma warning disable CS8618
    private PostGalleryId()
    {
    }
# pragma warning disable CS8618
}