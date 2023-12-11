using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate;

public sealed class Post : AggregateRoot<long>
{
    private readonly List<PostLike> _likes = new();
    private readonly List<PostGallery> _galleries = new();
    private readonly List<PostComment> _comments = new();
    
    public long UserId { get; private set; }
    public string Content { get; private set; }
    public int? LocationId { get; private set; }
    public int? Views { get; private set; }
    public bool HideStats { get; private set; }
    public bool HideComments { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<PostLike> Likes => _likes.AsReadOnly();
    public IReadOnlyList<PostGallery> Galleries => _galleries.AsReadOnly();
    public IReadOnlyList<PostComment> Comments => _comments.AsReadOnly();
    
    

    private Post(
        long userId,
        string content,
        int? locationId,
        int? views,
        bool hideStats,
        bool hideComments
        )
    {
        UserId = userId;
        Content = content;
        LocationId = locationId;
        Views = views;
        HideStats = hideStats;
        HideComments = hideComments;
    }

    public static Post Create(
        long userId,
        string content,
        int?locationId,
        int?views,
        bool hideStats,
        bool hideComments
        )
    {
        return new Post(
            userId,  
            content, 
            locationId,  
            views,   
            hideStats,   
            hideComments
        );
    }
    
    # pragma warning disable CS8618
    private Post()
    {
    }
    # pragma warning disable CS8618
}