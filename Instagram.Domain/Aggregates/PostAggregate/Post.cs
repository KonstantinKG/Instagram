using Instagram.Domain.Aggregates.LocationAggregate;
using Instagram.Domain.Aggregates.LocationAggregate.ValueObjects;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.PostAggregate.ValueObjects;
using Instagram.Domain.Aggregates.TagAggregate;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate;

public sealed class Post : AggregateRoot<PostId>
{
    private readonly List<UserId> _usersLikedIds = new();
    private readonly List<Tag> _tags = new();
    private readonly List<PostGallery> _galleries = new();
    private readonly List<PostComment> _comments = new();
    
    public UserId UserId { get; private set; }
    public User? User { get; private set;  }
    public string Content { get; private set; }
    public LocationId? LocationId { get; private set; }
    public Location? Location { get; private set; }
    public int? Views { get; private set; }
    public bool HideStats { get; private set; }
    public bool HideComments { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<UserId> UsersLikedIds => _usersLikedIds.AsReadOnly();
    public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();
    public IReadOnlyList<PostGallery> Galleries => _galleries.AsReadOnly();
    public IReadOnlyList<PostComment> Comments => _comments.AsReadOnly();
    
    

    private Post(
        PostId id,
        UserId userId,
        string content,
        LocationId? locationId,
        int? views,
        bool hideStats,
        bool hideComments
        )
    : base(id)
    {
        UserId = userId;
        Content = content;
        LocationId = locationId;
        Views = views;
        HideStats = hideStats;
        HideComments = hideComments;
    }

    public static Post Create(
        UserId userId,
        string content,
        LocationId? locationId,
        int? views,
        bool hideStats,
        bool hideComments
        )
    {
        return new Post(
            PostId.Create(), 
            userId,  
            content, 
            locationId,  
            views,   
            hideStats,   
            hideComments
        );
    }
    
    public static Post Fill(
        Guid id,
        UserId userId,
        string content,
        LocationId ?locationId,
        int ?views,
        bool hideStats,
        bool hideComments
        )
    {
        return new Post(
            PostId.Fill(id), 
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