using Instagram.Domain.Entities;

namespace Instagram.Application.Services.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token
    );