using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostCommentLike : ValueObject
{
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    
    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CommentId;
        yield return UserId;
    }
}