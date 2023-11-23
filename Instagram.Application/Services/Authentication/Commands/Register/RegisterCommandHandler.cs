using ErrorOr;
using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Application.Common.Interfaces.Persistence.CommandRepositories;
using Instagram.Application.Common.Interfaces.Persistence.QueryRepositories;
using Instagram.Application.Services.Authentication.Common;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.Authentication.Commands.Register;

public class RegisterCommandHandler
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IUserQueryRepository _userQueryRepository;

    public RegisterCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserQueryRepository userQueryRepository,
        IUserCommandRepository userCommandRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userQueryRepository = userQueryRepository;
        _userCommandRepository = userCommandRepository;
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
        
        var user = User.Create(
            username: command.Username,
            fullname: command.Fullname,
            email: command.Email,
            phone: null,
            password: command.Password,
            UserProfile.Empty()
        );
        
        await _userCommandRepository.AddUser(user);

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            token,
            "not implemented"
        );
    }
}