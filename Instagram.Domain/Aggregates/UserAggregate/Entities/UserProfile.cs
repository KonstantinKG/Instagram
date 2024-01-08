using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.Entities;

public class UserProfile : Entity<Guid>
{
    public Guid UserId { get; set; }
    
    public required string Fullname { get; set; }
    public string? Image { get; set; }
    public string? Bio { get; set; }
    public long GenderId { get; set; }
    public UserGender? Gender { get; set; }

    protected override IEnumerable<object?> GetDifferenceComponents()
    {
        yield return UserId;
        yield return Image;
        yield return Bio;
        yield return GenderId;
        yield return Gender;
    }
}