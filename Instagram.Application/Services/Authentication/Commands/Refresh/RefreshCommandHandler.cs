using ErrorOr;

using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence.TemporaryRepositories;
using Instagram.Application.Services.Authentication.Common;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.Authentication.Commands.Refresh;

public class RefreshCommandHandler
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IJwtTokenValidator _jwtTokenValidator;
    private readonly IJwtTokenHasher _jwtTokenHasher;
    private readonly IJwtTokenRepository _jwtTokenRepository;

    public RefreshCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator, 
        IJwtTokenRepository jwtTokenRepository,
        IJwtTokenValidator jwtTokenValidator,
        IJwtTokenHasher jwtTokenHasher)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _jwtTokenRepository = jwtTokenRepository;
        _jwtTokenValidator = jwtTokenValidator;
        _jwtTokenHasher = jwtTokenHasher;
    }
    
    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RefreshCommand command,
        CancellationToken cancellationToken)
    {
        var refreshTokenPrincipal = _jwtTokenValidator.ValidateToken(command.Token, out var validatedRefreshToken);
        
        if (refreshTokenPrincipal is null)
            return Errors.Auth.InvalidToken;
        
        
        var oldTokenHash = _jwtTokenHasher.HashToken(command.Token);
        var oldToken = await _jwtTokenRepository.GetToken(oldTokenHash);
        if (oldToken is null)
        {
            var sessionId = refreshTokenPrincipal.Claims.First(x => x.Type == "sid").Value;
            await _jwtTokenRepository.DeleteAllSessionTokens(sessionId);
            return Errors.Auth.InvalidToken;
        }

        var tokenParameters = new TokenParameters(
            refreshTokenPrincipal.Claims.First(x => x.Type == "nameid").Value,
            refreshTokenPrincipal.Claims.First(x => x.Type == "sid").Value,
            refreshTokenPrincipal.Claims.First(x => x.Type == "unique_name").Value,
            refreshTokenPrincipal.Claims.First(x => x.Type == "email").Value
        );
        
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(tokenParameters);
        var refreshToken = _jwtTokenGenerator.RotateRefreshToken(tokenParameters, validatedRefreshToken!);

        var userId = tokenParameters.Id;
        var newTokenHash = _jwtTokenHasher.HashToken(refreshToken);
        
        await _jwtTokenRepository.DeleteToken(oldTokenHash);
        await _jwtTokenRepository.InsertToken(
            userId,
            tokenParameters.SessionId,
            newTokenHash
        );

        return new AuthenticationResult(
            accessToken,
            refreshToken
        );
    }
}