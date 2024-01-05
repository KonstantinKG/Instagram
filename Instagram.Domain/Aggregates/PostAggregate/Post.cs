using Instagram.Domain.Aggregates.LocationAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.TagAggregate;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate;

public sealed class Post : AggregateRoot<Guid>
{
    private readonly List<PostLike> _postLikes = new();
    private readonly List<Tag> _tags = new();
    private readonly List<PostGallery> _galleries = new();
    private readonly List<PostComment> _comments = new();
    
    public Guid UserId { get; init; }
    public User? User { get; set; }
    public string? Content { get; init; }
    public long? LocationId { get; init; }
    public Location? Location { get; init; }
    public int? Views { get; init; }
    public bool HideStats { get; init; }
    public bool HideComments { get; init; }
    public bool Active { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    
    public IReadOnlyList<PostLike> PostLikes => _postLikes.AsReadOnly();
    public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();
    public IReadOnlyList<PostGallery> Galleries => _galleries.AsReadOnly();
    public IReadOnlyList<PostComment> Comments => _comments.AsReadOnly();
    
    public long CommentsCount { get; set; }
    public long LikesCount { get; set; }

    public static Post Create(Guid userId)
    {
        return new Post() {
            Id = Guid.NewGuid(),
            UserId = userId,
            Content = null,
            LocationId = null,
            Views = 0,
            HideStats = false,
            HideComments = false,
            Active = false
        };
    }

    public void AddGallery(PostGallery gallery)
    {
        _galleries.Add(gallery);
    }
    
    public void AddComment(PostComment comment)
    {
        _comments.Add(comment);
    }
    
    public void AddLike(PostLike like)
    {
        _postLikes.Add(like);
    } 
    
    public void AddTag(Tag tag)
    {
        _tags.Add(tag);
    }

    protected override IEnumerable<object?> GetDifferenceComponents()
    {
        yield return Content;
        yield return LocationId;
        yield return HideStats;
        yield return HideComments;
    }
}