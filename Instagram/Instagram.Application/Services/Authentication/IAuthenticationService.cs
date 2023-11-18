namespace Instagram.Application.Services.Authentication;

public interface IAuthenticationService
{
    public AuthenticationResult Login();
    public AuthenticationResult Register();
}