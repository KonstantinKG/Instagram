using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostGallery : Entity<Guid>
{
    public Guid PostId { get; private set; }
    public string File { get; private set; }
    public string? Description { get; private set; }
    public string? Labels { get; private set; }

    private PostGallery(
        Guid id,
        Guid postId,
        string file,
        string? description,
        string? labels
        )
    : base(id)
    {
        PostId = postId;
        File = file;
        Description = description;
        Labels = labels;
    }
    
    public static PostGallery Create(
        Guid postId,
        string file,
        string? description,
        string? labels
        )
    {
        return new PostGallery(
            Guid.NewGuid(), 
            postId,
            file,
            description,
            labels
            );
    }
    
    public static PostGallery Fill(
        Guid id,
        Guid postId,
        string file,
        string? description,
        string? labels
        )
    {
        return new PostGallery(
            id, 
            postId,
            file,
            description,
            labels
            );
    }
    
    protected override IEnumerable<object?> GetDifferenceComponents()
    {
        yield return File;
        yield return Description;
        yield return Labels;
    }
    
# pragma warning disable CS8618
    private PostGallery()
    {
    }
# pragma warning disable CS8618
}