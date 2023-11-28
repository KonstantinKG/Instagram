using System.Security.Cryptography;
using System.Text;

using ErrorOr;

using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence.QueryRepositories;
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
        var principal = _jwtTokenValidator.ValidateToken(command.Token, out var validatedToken);
        if (principal is null)
        {
            return Errors.Auth.InvalidToken;
        }

        var claimsSessionId = principal.Claims.FirstOrDefault(x => x.Type == "sid")?.Value;
        var claimsUserId = principal.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;
        var claimsUsername = principal.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
        var claimsEmail = principal.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
        if (
            claimsUserId is null ||
            claimsSessionId is null ||
            claimsUsername is null ||
            claimsEmail is null
            )
        {
            return Errors.Auth.InvalidToken;
        }
        
        var tokenHash = _jwtTokenHasher.HashToken(command.Token);
        var token = await _jwtTokenRepository.GetToken(tokenHash);
        if (token is null)
        {
            await _jwtTokenRepository.DeleteAllSessionTokens(claimsSessionId);
            return Errors.Auth.InvalidToken;
        }
        
        var tokenParameters = new TokenParameters(
            claimsUserId,
            claimsSessionId,
            claimsUsername,
            claimsEmail
        );
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(tokenParameters);
        var refreshToken = _jwtTokenGenerator.GenerateToken(tokenParameters, validatedToken!.ValidTo);
        var newTokenHash = _jwtTokenHasher.HashToken((refreshToken));
        
        await _jwtTokenRepository.DeleteToken(tokenHash);

        var userId = long.Parse(claimsUserId);
        await _jwtTokenRepository.InsertToken(
            userId,
            claimsSessionId,
            newTokenHash
        );

        return new AuthenticationResult(
            accessToken,
            refreshToken
        );
    }
}