using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Services.UserService.Queries.GetAllUsers;

public record GetAllUsersResult(
    long Current,
    long Total,
    List<User> Users
    );