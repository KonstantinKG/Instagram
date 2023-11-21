using ErrorOr;
using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Domain.Common.Errors;
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
    
    public ErrorOr<AuthenticationResult> Login(LoginCommand command)
    {
        if (_userRepository.GetUserByEmail(command.Email) is not User user)
        {
            return Errors.User.InvalidCredentials;
        }
        
        if (user.Password != command.Password)
        {
            return Errors.User.InvalidCredentials;
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            user,
            token
        );
    }

    public ErrorOr<AuthenticationResult> Register(RegisterCommand command)
    {
        if (_userRepository.GetUserByEmail(command.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }
        
        var userId = Guid.NewGuid();
        var user = new User()
        {
            Id = userId,
            Name = command.Name,
            Email = command.Email,
            Password = command.Password
        };
        
        _userRepository.AddUser(user);
        

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            user,
            token
        );
    }
}