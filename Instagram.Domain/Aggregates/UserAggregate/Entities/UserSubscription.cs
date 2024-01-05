using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.Entities;

public class UserSubscription : ValueObject
{
    public Guid SubscriberId { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return SubscriberId;
        yield return UserId;
    }
}