using Instagram.Domain.Aggregates.UserAggregate.Entities;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate;

public sealed class User : AggregateRoot<Guid>
{
    public required string Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public required string Password { get; set; }
    
    public required UserProfile Profile { get; set; }

    protected override IEnumerable<object?> GetDifferenceComponents()
    {
        yield return Username;
        yield return Email;
        yield return Phone;
        yield return Password;
    }
}