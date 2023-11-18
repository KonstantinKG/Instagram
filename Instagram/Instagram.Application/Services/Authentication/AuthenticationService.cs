using Instagram.Application.Common.Interfaces.Authentication;

namespace Instagram.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public AuthenticationResult Login()
    {
        var userId = Guid.NewGuid();
        var name = "mock-name";
        var token = _jwtTokenGenerator.GenerateToken(userId, name);
        
        return new AuthenticationResult(
            Guid.NewGuid(),
            name,
            "mock-email@mock.com",
            token
        );
    }

    public AuthenticationResult Register()
    {   
        var userId = Guid.NewGuid();
        var name = "mock-name";
        
        return new AuthenticationResult(
            Guid.NewGuid(),
            name,
            "mock-email@mock.com",
            "mock-bearer-token"
        );
    }
}