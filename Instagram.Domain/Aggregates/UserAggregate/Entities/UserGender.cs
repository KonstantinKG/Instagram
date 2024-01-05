using Instagram.Domain.Common.Models;

namespace Instagram.Domain.Aggregates.UserAggregate.Entities;

public class UserGender : Entity<long>
{
    public required string Name { get; init; }
}