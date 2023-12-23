using Instagram.Domain.Aggregates.PostAggregate.ValueObjects;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostCommentLike : ValueObject
{
    public PostCommentId CommentId { get; private set; }
    public UserId UserId { get; private set; }
    

    private PostCommentLike(
        UserId userId,
        PostCommentId postCommentId
            )
    {
        UserId = userId;
        CommentId = postCommentId;
    }
    
    public static PostCommentLike Create(
        UserId userId,
        PostCommentId postId
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