using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Services.UserService.Queries.GetAllUsers;

public record GetAllUsersQuery(
    int Page
    );