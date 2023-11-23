using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Services.UserService.Queries.GetUser;

public record GetUserResult(
    User User
    );