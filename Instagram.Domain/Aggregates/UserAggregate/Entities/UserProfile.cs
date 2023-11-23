using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.Entities;

public class UserProfile : Entity<long>
{
    public string? Image { get; private set; }
    public string? Bio { get; private set; }

    private UserProfile(
        string? image,
        string? bio)
    {
        Image = image;
        Bio = bio;
    }
    
    public static UserProfile Create(
        string? image,
        string? bio)
    {
        return new UserProfile(
            image,
            bio
            );
    }
    
    public static UserProfile Empty()
    {
        return new UserProfile(
            null,
            null
        );
    }
    
# pragma warning disable CS8618
    private UserProfile()
    {
    }
# pragma warning disable CS8618
}