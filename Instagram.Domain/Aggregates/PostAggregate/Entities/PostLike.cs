using Instagram.Domain.Aggregates.PostAggregate.ValueObjects;

using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostLike : ValueObject
{
    public PostId PostId { get; private set; }
    public UserId UserId { get; private set; }
    

    private PostLike(
        UserId userId,
        PostId postId
            )
    {
        UserId = userId;
        PostId = postId;
    }
    
    public static PostLike Create(
        UserId userId,
        PostId postId
        )
    {
        return new PostLike(
            userId,
            postId
            );
    }
    
    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PostId;
        yield return UserId;
    }
    
# pragma warning disable CS8618
    private PostLike()
    {
    }
# pragma warning disable CS8618

}