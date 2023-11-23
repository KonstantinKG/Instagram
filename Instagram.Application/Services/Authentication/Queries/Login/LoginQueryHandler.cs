using ErrorOr;
using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence.QueryRepositories;
using Instagram.Application.Services.Authentication.Common;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.Authentication.Queries.Login;

public class LoginQueryHandler
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserQueryRepository _userQueryRepository;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserQueryRepository userQueryRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userQueryRepository = userQueryRepository;
    }
    
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        if (await _userQueryRepository.GetUserByIdentity(
                query.Identity,
                query.Identity,
                query.Identity)
            is not User user)
        {
            return Errors.User.InvalidCredentials;
        }
        
        if (user.Password != query.Password)
        {
            return Errors.User.InvalidCredentials;
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            token,
            "not implemented"
        );
    }
}