namespace Instagram.Application.Services.Authentication;

public interface IAuthenticationService
{
    public AuthenticationResult Login(string name, string password);
    public AuthenticationResult Register(string name, string email, string password);
}