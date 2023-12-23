using System.Security.Cryptography;
using System.Text;

using ErrorOr;
using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.TemporaryRepositories;
using Instagram.Application.Services.Authentication.Common;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Errors;

using Microsoft.AspNetCore.Identity;

namespace Instagram.Application.Services.Authentication.Queries.Login;

public class LoginQueryHandler
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDapperUserRepository _dapperUserRepository;
    private readonly IJwtTokenRepository _jwtTokenRepository;
    private readonly IJwtTokenHasher _jwtTokenHasher;
    private readonly IPasswordHasher<object> _passwordHasher;

    public LoginQueryHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IDapperUserRepository dapperUserRepository,
        IJwtTokenRepository jwtTokenRepository,
        IPasswordHasher<object> passwordHasher,
        IJwtTokenHasher jwtTokenHasher)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _dapperUserRepository = dapperUserRepository;
        _jwtTokenRepository = jwtTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenHasher = jwtTokenHasher;
    }
    
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        if (await _dapperUserRepository.GetUserByIdentity(
                query.Identity,
                query.Identity,
                query.Identity)
            is not User user)
        {
            return Errors.User.InvalidCredentials;
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(new object(), user.Password, query.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return Errors.User.InvalidCredentials;
        }

        var userSessionId = Guid.NewGuid().ToString();
        var tokenParameters = new TokenParameters(
            user.Id.Value.ToString(),
            userSessionId,
            user.Username,
            user.Email
        );
        
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(tokenParameters);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(tokenParameters);

        var tokenHash = _jwtTokenHasher.HashToken(refreshToken);
        await _jwtTokenRepository.InsertToken(user.Id.Value.ToString(), userSessionId, tokenHash);
        
        return new AuthenticationResult(
            accessToken,
            refreshToken
        );
    }
}