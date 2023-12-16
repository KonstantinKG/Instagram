using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.Entities;

public class UserProfile : Entity<UserProfileId>
{
    public string? Image { get; private set; }
    public string? Bio { get; private set; }

    private UserProfile(
        UserProfileId id,
        string? image,
        string? bio
        )
    : base(id)
    {
        Image = image;
        Bio = bio;
    }
    
    public static UserProfile Create(
        string? image,
        string? bio
        )
    {
        return new UserProfile(
            UserProfileId.Create(),
            image,
            bio
        );
    }
    
    public static UserProfile Fill(
        Guid id,
        string? image,
        string? bio
        )
    {
        return new UserProfile(
            UserProfileId.Fill(id),
            image,
            bio
        );
    }
    
# pragma warning disable CS8618
    private UserProfile()
    {
    }
# pragma warning disable CS8618
}