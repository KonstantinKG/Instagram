using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostLike : Entity<long>
{
    public long UserId { get; private set; }
    public long PostId { get; private set; }

    private PostLike(
        long userId,
        long postId
        )
    {
        UserId = userId;
        PostId = postId;
    }
    
    public static PostLike Create(
        long userId,
        long postId
        )
    {
        return new PostLike(
            userId,
            postId
            );
    }
    
# pragma warning disable CS8618
    private PostLike()
    {
    }
# pragma warning disable CS8618
}