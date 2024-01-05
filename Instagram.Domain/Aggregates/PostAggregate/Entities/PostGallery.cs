using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostGallery : Entity<Guid>
{
    public Guid PostId { get; set; }
    public required string File { get; set; }
    public string? Description { get; set; }
    public string? Labels { get; set; }
    
    protected override IEnumerable<object?> GetDifferenceComponents()
    {
        yield return File;
        yield return Description;
        yield return Labels;
    }
}