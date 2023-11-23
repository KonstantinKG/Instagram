using ErrorOr;
using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Application.Services.Authentication.Common;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.Authentication.Commands.Register;

public class RegisterCommandHandler
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetUserBy(u=> 
                    u.Username == command.Username ||
                    u.Email == command.Email) is User existingUser)
        {
            List<Error> errors = new ();
            if (existingUser.Username == command.Username) 
                errors.Add(Error.Conflict(description: "Пользователь с данным именем уже существует."));
            if (existingUser.Email == command.Email) 
                errors.Add(Error.Conflict(description: "Пользователь с данной почтой уже существует."));
            return errors;
        }
        
        var user = User.Create(
            username: command.Username,
            fullname: command.Fullname,
            email: command.Email,
            phone: null,
            password: command.Password
        );
        
        await _userRepository.AddUser(user);
        

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            user,
            token
        );
    }
}