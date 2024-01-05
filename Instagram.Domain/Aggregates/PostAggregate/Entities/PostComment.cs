using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostComment : Entity<Guid>
{
    private readonly List<PostComment> _comments = new();
    private readonly List<PostCommentLike> _commentLikes = new();
    
    public Guid PostId { get; set; }
    public Guid? ParentId { get; set; }
    public string? Content { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public IReadOnlyList<PostComment> Comments => _comments.AsReadOnly();
    public IReadOnlyList<PostCommentLike> CommentLikes => _commentLikes.AsReadOnly();

    
    public void AddChild(PostComment comment)
    {
        _comments.Add(comment);
    }
    
    public void AddLike(PostCommentLike like)
    {
        _commentLikes.Add(like);
    }
}