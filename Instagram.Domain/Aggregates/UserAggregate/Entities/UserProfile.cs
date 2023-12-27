using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.Entities;

public class UserProfile : Entity<Guid>
{
    
    public Guid UserId { get; private set; }
    public string? Image { get; private set; }
    public string? Bio { get; private set; }
    public long GenderId { get; set; }
    public UserGender? Gender { get; set; }

    private UserProfile(
        Guid id,
        Guid userId,
        string? image,
        string? bio,
        UserGender? gender
        )
    : base(id)
    {
        UserId = userId;
        Image = image;
        Bio = bio;
        Gender = gender;
    }
    
    public static UserProfile Create(
        Guid userId,
        string? image,
        string? bio,
        UserGender? gender
        )
    {
        return new UserProfile(
            Guid.NewGuid(),
            userId,
            image,
            bio,
            gender
        );
    }
    
    public static UserProfile Fill(
        Guid id,
        Guid userId,
        string? image,
        string? bio,
        UserGender? sex
    )
    {
        return new UserProfile(
            id,
            userId,
            image,
            bio,
            sex
        );
    }

    protected override IEnumerable<object?> GetDifferenceComponents()
    {
        yield return UserId;
        yield return Image;
        yield return Bio;
        yield return GenderId;
        yield return Gender;
    }

# pragma warning disable CS8618
    private UserProfile()
    {
    }
# pragma warning disable CS8618
}