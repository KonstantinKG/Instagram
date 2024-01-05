using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostLike : ValueObject
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    
    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PostId;
        yield return UserId;
    }
}