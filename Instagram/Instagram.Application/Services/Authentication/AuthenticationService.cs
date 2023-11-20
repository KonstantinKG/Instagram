using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Domain.Entities;

namespace Instagram.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }
    
    public AuthenticationResult Login(string email, string password)
    {
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            throw new Exception("User with given email already exists");
        }
        
        if (user.Password != password)
        {
            throw new Exception("Password is incorrect");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            user,
            token
        );
    }

    public AuthenticationResult Register(string name, string email, string password)
    {
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            throw new Exception("User with given email already exists");
        }
        
        var userId = Guid.NewGuid();
        var user = new User()
        {
            Id = userId,
            Name = name,
            Email = email,
            Password = password
        };
        
        _userRepository.AddUser(user);
        

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            user,
            token
        );
    }
}