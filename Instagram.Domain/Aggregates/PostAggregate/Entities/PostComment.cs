using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostComment : Entity<long>
{
    public long PostId { get; private set; }
    public long UserId { get; private set; }
    public long ParentId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private PostComment(
        long postId,
        long userId,
        long parentId,
        string content
        )
    {
        PostId = postId;
        UserId = userId;
        ParentId = parentId;
        Content = content;
    }
    
    public static PostComment Create(
        long postId,
        long userId,
        long parentId,
        string content
        )
    {
        return new PostComment(
            postId,
            userId,
            parentId,
            content
            );
    }
    
# pragma warning disable CS8618
    private PostComment()
    {
    }
# pragma warning disable CS8618
}