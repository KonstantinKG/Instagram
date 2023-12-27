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
    
    public Guid UserId { get; private set; }
    public User? User { get; private set;  }
    public string? Content { get; private set; }
    public long? LocationId { get; private set; }
    public Location? Location { get; private set; }
    public int? Views { get; private set; }
    public bool HideStats { get; private set; }
    public bool HideComments { get; private set; }
    public bool Active { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    public IReadOnlyList<PostLike> PostLikes => _postLikes.AsReadOnly();
    public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();
    public IReadOnlyList<PostGallery> Galleries => _galleries.AsReadOnly();
    public IReadOnlyList<PostComment> Comments => _comments.AsReadOnly();
    
    

    private Post(
        Guid id,
        Guid userId,
        string? content,
        long? locationId,
        int? views,
        bool hideStats,
        bool hideComments,
        bool active
        )
    : base(id)
    {
        UserId = userId;
        Content = content;
        LocationId = locationId;
        Views = views;
        HideStats = hideStats;
        HideComments = hideComments;
        Active = active;
    }

    public static Post Create(Guid userId)
    {
        return new Post(
            Guid.NewGuid(), 
            userId,  
            null, 
            null,  
            null,   
            false,   
            false,
            false
        );
    }
    
    public static Post Fill(
        Guid id,
        Guid userId,
        string? content,
        long? locationId,
        int ?views,
        bool hideStats,
        bool hideComments,
        bool active
        )
    {
        return new Post(
            id, 
            userId,  
            content, 
            locationId,  
            views,   
            hideStats,   
            hideComments,
            active
        );
    }

    protected override IEnumerable<object?> GetDifferenceComponents()
    {
        yield return Content;
        yield return LocationId;
        yield return HideStats;
        yield return HideComments;
    }

# pragma warning disable CS8618
    private Post()
    {
    }
    # pragma warning disable CS8618
}