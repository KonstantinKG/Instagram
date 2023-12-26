using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.Entities;

public class UserGender : Entity<long>
{
    public string Name { get; private set; }

    private UserGender(
        string name
        )
    {
        Name = name;
    }

    public static UserGender Create(
        string name
        )
    {
        return new UserGender(
            name
        );
    }
    
# pragma warning disable CS8618
    private UserGender()
    {
    }
# pragma warning disable CS8618
}