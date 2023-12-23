using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.Entities;

public class UserProfile : Entity<UserProfileId>
{
    
    public UserId UserId { get; private set; }
    public string? Image { get; private set; }
    public string? Bio { get; private set; }
    
    public Gender? Sex { get; private set; }

    private UserProfile(
        UserProfileId id,
        UserId userId,
        string? image,
        string? bio,
        Gender? sex
        )
    : base(id)
    {
        UserId = userId;
        Image = image;
        Bio = bio;
        Sex = sex;
    }
    
    public static UserProfile Create(
        UserId userId,
        string? image,
        string? bio,
        Gender? sex
        )
    {
        return new UserProfile(
            UserProfileId.Create(),
            userId,
            image,
            bio,
            sex
        );
    }
    
    public static UserProfile Fill(
        UserProfileId id,
        UserId userId,
        string? image,
        string? bio,
        Gender? sex
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
    
# pragma warning disable CS8618
    private UserProfile()
    {
    }
# pragma warning disable CS8618
}