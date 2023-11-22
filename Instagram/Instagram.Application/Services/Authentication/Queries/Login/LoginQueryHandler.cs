using ErrorOr;
using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Application.Services.Authentication.Commands.Register;
using Instagram.Application.Services.Authentication.Common;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Entities;

namespace Instagram.Application.Services.Authentication.Queries.Login;

public class LoginQueryHandler
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }
    
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        if (_userRepository.GetUserByEmail(query.Email) is not User user)
        {
            return Errors.User.InvalidCredentials;
        }
        
        if (user.Password != query.Password)
        {
            return Errors.User.InvalidCredentials;
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            user,
            token
        );
    }
}