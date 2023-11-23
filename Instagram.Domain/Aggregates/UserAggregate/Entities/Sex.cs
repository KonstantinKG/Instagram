using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.Entities;

public class Sex : Entity<int>
{
    public string Name { get; private set; }

    private Sex(string name)
    {
        Name = name;
    }

    public static Sex Create(string name)
    {
        return new Sex(name);
    }
    
# pragma warning disable CS8618
    private Sex()
    {
    }
# pragma warning disable CS8618
}