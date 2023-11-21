using ErrorOr;

namespace Instagram.Application.Services.Authentication;

public interface IAuthenticationService
{
    public ErrorOr<AuthenticationResult> Login(LoginCommand command);
    public ErrorOr<AuthenticationResult> Register(RegisterCommand command);
}