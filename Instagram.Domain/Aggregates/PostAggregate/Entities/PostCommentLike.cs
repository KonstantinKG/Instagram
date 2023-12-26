using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostCommentLike : ValueObject
{
    public Guid CommentId { get; private set; }
    public Guid UserId { get; private set; }
    

    private PostCommentLike(
        Guid userId,
        Guid postCommentId
            )
    {
        UserId = userId;
        CommentId = postCommentId;
    }
    
    public static PostCommentLike Create(
        Guid userId,
        Guid postId
        )
    {
        return new PostCommentLike(
            userId,
            postId
            );
    }
    
    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CommentId;
        yield return UserId;
    }
    
# pragma warning disable CS8618
    private PostCommentLike()
    {
    }
# pragma warning disable CS8618
}