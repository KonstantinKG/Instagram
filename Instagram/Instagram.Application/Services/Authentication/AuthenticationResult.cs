using Instagram.Domain.Entities;

namespace Instagram.Application.Services.Authentication;

public record AuthenticationResult(
    User User,
    string Token
    );