using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.PostAggregate.Entities;

public class PostToTag : ValueObject
{
    public Guid PostId { get; set; }
    public Guid TagId { get; set; }
    
    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PostId;
        yield return TagId;
    }

}