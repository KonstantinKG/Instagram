using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;
using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.Entities;

public class Gender : Entity<long>
{
    public string Name { get; private set; }

    private Gender(
        string name
        )
    {
        Name = name;
    }

    public static Gender Create(
        string name
        )
    {
        return new Gender(
            name
        );
    }
    
# pragma warning disable CS8618
    private Gender()
    {
    }
# pragma warning disable CS8618
}