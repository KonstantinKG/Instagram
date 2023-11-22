using ErrorOr;
using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Application.Services.Authentication.Common;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Entities;

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
        await Task.CompletedTask;
        
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