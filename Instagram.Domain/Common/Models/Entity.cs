namespace Instagram.Domain.Common.Models;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    public TId Id { get; init; }
    
    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }

    protected virtual IEnumerable<object?> GetDifferenceComponents()
    {
        yield return Id;
    }
    
    public bool Different(Entity<TId> obj)
    {
        return !GetDifferenceComponents().SequenceEqual(obj.GetDifferenceComponents());
    }

    public bool Equals(Entity<TId>? other)
    {
        return Equals((object?)other);
    }

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    # pragma warning disable CS8618
        protected Entity()
        {
        }
    # pragma warning disable CS8618
}