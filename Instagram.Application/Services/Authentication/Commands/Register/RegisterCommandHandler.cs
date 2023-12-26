using ErrorOr;
using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Application.Common.Interfaces.Persistence.TemporaryRepositories;
using Instagram.Application.Services.Authentication.Common;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;
using Instagram.Domain.Common.Errors;

using Microsoft.AspNetCore.Identity;

namespace Instagram.Application.Services.Authentication.Commands.Register;

public class RegisterCommandHandler
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IJwtTokenHasher _jwtTokenHasher;
    private readonly IEfUserRepository _efUserRepository;
    private readonly IDapperUserRepository _dapperUserRepository;
    private readonly IJwtTokenRepository _jwtTokenRepository;
    private readonly IPasswordHasher<object> _passwordHasher;

    public RegisterCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IDapperUserRepository dapperUserRepository,
        IEfUserRepository efUserRepository,
        IJwtTokenRepository jwtTokenRepository, 
        IPasswordHasher<object> passwordHasher,
        IJwtTokenHasher jwtTokenHasher)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _dapperUserRepository = dapperUserRepository;
        _efUserRepository = efUserRepository;
        _jwtTokenRepository = jwtTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenHasher = jwtTokenHasher;
    }
    
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        
        if (await _dapperUserRepository.GetUserByIdentity(
                command.Username,
                command.Email,
                null)
            is User existingUser)
        {
            List<Error> errors = new ();
            
            if (existingUser.Username == command.Username) 
                errors.Add(Errors.User.UniqueUsername);
            
            if (existingUser.Email == command.Email) 
                errors.Add(Errors.User.UniqueEmail);
            return errors;
        }

        var userId = Guid.NewGuid();
        var passwordHash = _passwordHasher.HashPassword(new object(), command.Password);

        UserGender? gender = null;
        if (command.Gender != null)
        {
            gender = await _dapperUserRepository.GetUserGender(command.Gender);
            gender ??= UserGender.Create(command.Gender);
        }
        
        var profile = UserProfile.Create(
            userId,
            null,
            null,
            gender
        );
        var user = User.Fill(
            userId,
            command.Username,
            command.Fullname,
            command.Email,
            null,
            passwordHash,
            profile
        );
        
        await _efUserRepository.AddUser(user);
        
        var userSessionId = Guid.NewGuid().ToString();
        var tokenParameters = new TokenParameters(
            user.Id.ToString(),
            userSessionId,
            user.Username,
            user.Email
        );
        
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(tokenParameters);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(tokenParameters);

        var tokenHash = _jwtTokenHasher.HashToken(refreshToken);
        await _jwtTokenRepository.InsertToken(user.Id.ToString(), userSessionId, tokenHash);
        
        return new AuthenticationResult(
            accessToken,
            refreshToken
        );
    }
}