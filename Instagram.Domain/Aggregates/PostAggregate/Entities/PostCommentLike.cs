using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostCommentLike : Entity<long>
{
    public long UserId { get; private set; }
    public long CommentId { get; private set; }

    private PostCommentLike(
        long userId,
        long commentId
        )
    {
        UserId = userId;
        CommentId = commentId;
    }
    
    public static PostCommentLike Create(
        long userId,
        long commentId
        )
    {
        return new PostCommentLike(
            userId,
            commentId
            );
    }
    
# pragma warning disable CS8618
    private PostCommentLike()
    {
    }
# pragma warning disable CS8618
}