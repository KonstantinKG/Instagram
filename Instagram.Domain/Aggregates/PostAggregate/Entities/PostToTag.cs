using Instagram.Domain.Aggregates.PostAggregate.ValueObjects;
using Instagram.Domain.Aggregates.TagAggregate.ValueObjects;
using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostToTag : ValueObject
{
    public PostId PostId { get; private set; }
    public TagId TagId { get; private set; }
    

    private PostToTag(
        TagId tagId,
        PostId postId
            )
    {
        TagId = tagId;
        PostId = postId;
    }
    
    public static PostToTag Create(
        TagId tagId,
        PostId postId
        )
    {
        return new PostToTag(
            tagId,
            postId
            );
    }
    
    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PostId;
        yield return TagId;
    }
    
# pragma warning disable CS8618
    private PostToTag()
    {
    }
# pragma warning disable CS8618

}