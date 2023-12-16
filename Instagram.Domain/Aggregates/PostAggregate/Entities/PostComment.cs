using Instagram.Domain.Aggregates.PostAggregate.ValueObjects;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostComment : Entity<PostCommentId>
{
    private readonly List<PostComment> _comments = new();
    private readonly List<UserId> _usersLikedIds = new();
    
    public PostCommentId? ParentId { get; private set; }
    public string Content { get; private set; }
    
    public UserId UserId { get; private set; }
    public User? User { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public IReadOnlyList<PostComment> Comments => _comments.AsReadOnly();
    public IReadOnlyList<UserId> UsersLikedIds => _usersLikedIds.AsReadOnly();

    private PostComment(
        PostCommentId id,
        PostCommentId? parentId,
        UserId userId,
        string content)
    : base(id)
    {
        UserId = userId;
        ParentId = parentId;
        Content = content;
    }
    
    public static PostComment Create(
        PostCommentId? parentId,
        UserId userId,
        string content
        )
    {
        return new PostComment(
            PostCommentId.Create(),
            parentId,
            userId,
            content
            );
    }
    
    public static PostComment Fill(
        Guid id,
        PostCommentId? parentId,
        UserId userId,
        string content
    )
    {
        return new PostComment(
            PostCommentId.Fill(id),
            parentId,
            userId,
            content
        );
    }
    
# pragma warning disable CS8618
    private PostComment()
    {
    }
# pragma warning disable CS8618
}