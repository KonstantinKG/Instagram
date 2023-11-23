using Instagram.Domain.Aggregates.UserAggregate;

namespace Instagram.Application.Services.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token
    );