using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostLike : ValueObject
{
    public Guid PostId { get; private set; }
    public Guid UserId { get; private set; }
    

    private PostLike(
        Guid userId,
        Guid postId
            )
    {
        UserId = userId;
        PostId = postId;
    }
    
    public static PostLike Create(
        Guid userId,
        Guid postId
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