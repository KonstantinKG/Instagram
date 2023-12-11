using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostGallery : Entity<long>
{
    public long PostId { get; private set; }
    public string File { get; private set; }
    public string? Description { get; private set; }
    public string? Labels { get; private set; }

    private PostGallery(
        long postId,
        string file,
        string? description,
        string? labels
        )
    {
        PostId = postId;
        File = file;
        Description = description;
        Labels = labels;
    }
    
    public static PostGallery Create(
        long postId,
        string file,
        string? description,
        string? labels
        )
    {
        return new PostGallery(
            postId,
            file,
            description,
            labels
            );
    }
    
# pragma warning disable CS8618
    private PostGallery()
    {
    }
# pragma warning disable CS8618
}