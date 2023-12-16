using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.ValueObjects;

public sealed class PostCommentId : ValueObject
{
    public Guid Value { get; }
    
    private PostCommentId(Guid value)
    {
        Value = value;
    }
    
    

    public static PostCommentId Create() => new(Guid.NewGuid());
    public static PostCommentId Fill(Guid guid) => new(guid);

    public override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
    
# pragma warning disable CS8618
    private PostCommentId()
    {
    }
# pragma warning disable CS8618
}