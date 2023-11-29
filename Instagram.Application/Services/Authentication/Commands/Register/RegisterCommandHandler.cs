using System.Security.Cryptography;
using System.Text;

using ErrorOr;
using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Application.Common.Interfaces.Persistence.CommandRepositories;
using Instagram.Application.Common.Interfaces.Persistence.QueryRepositories;
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
    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IJwtTokenRepository _jwtTokenRepository;
    private readonly IPasswordHasher<object> _passwordHasher;

    public RegisterCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserQueryRepository userQueryRepository,
        IUserCommandRepository userCommandRepository,
        IJwtTokenRepository jwtTokenRepository, 
        IPasswordHasher<object> passwordHasher,
        IJwtTokenHasher jwtTokenHasher)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userQueryRepository = userQueryRepository;
        _userCommandRepository = userCommandRepository;
        _jwtTokenRepository = jwtTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenHasher = jwtTokenHasher;
    }
    
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        
        if (await _userQueryRepository.GetUserByIdentity(
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

        var passwordHash = _passwordHasher.HashPassword(new object(), command.Password);
        var user = User.Create(
            username: command.Username,
            fullname: command.Fullname,
            email: command.Email,
            phone: null,
            password: passwordHash,
            UserProfile.Empty()
        );
        
        await _userCommandRepository.AddUser(user);
        
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
        await _jwtTokenRepository.InsertToken(user.Id, userSessionId, tokenHash);
        
        return new AuthenticationResult(
            accessToken,
            refreshToken
        );
    }
}