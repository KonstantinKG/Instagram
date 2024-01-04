using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostComment : Entity<Guid>
{
    private readonly List<PostComment> _comments = new();
    private readonly List<PostCommentLike> _commentLikes = new();
    
    public Guid PostId { get; private set; }
    public Guid? ParentId { get; private set; }
    public string Content { get; private set; }
    
    public Guid UserId { get; private set; }
    public User? User { get; set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public IReadOnlyList<PostComment> Comments => _comments.AsReadOnly();
    public IReadOnlyList<PostCommentLike> CommentLikes => _commentLikes.AsReadOnly();

    private PostComment(
        Guid id,
        Guid postId,
        Guid? parentId,
        Guid userId,
        string content)
    : base(id)
    {
        PostId = postId;
        UserId = userId;
        ParentId = parentId;
        Content = content;
    }
    
    public static PostComment Create(
        Guid postId,
        Guid? parentId,
        Guid userId,
        string content
        )
    {
        return new PostComment(
            Guid.NewGuid(),
            postId,
            parentId,
            userId,
            content
            );
    }
    
    public static PostComment Fill(
        Guid id,
        Guid postId,
        Guid? parentId,
        Guid userId,
        string content
    )
    {
        return new PostComment(
            id,
            postId,
            parentId,
            userId,
            content
        );
    }

    public void AddChild(PostComment comment)
    {
        _comments.Add(comment);
    }
    
# pragma warning disable CS8618
    private PostComment()
    {
    }
# pragma warning disable CS8618
}