using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.CommandRepositories;
using Instagram.Application.Common.Interfaces.Persistence.QueryRepositories;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.UserService.Commands;

public class EditUserCommandHandler
{
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IFileDownloader _fileDownloader;
    
    public EditUserCommandHandler(
        IUserQueryRepository userQueryRepository,
        IUserCommandRepository userCommandRepository,
        IFileDownloader fileDownloader)
    {
        _userQueryRepository = userQueryRepository;
        _userCommandRepository = userCommandRepository;
        _fileDownloader = fileDownloader;
    }

    public async Task<ErrorOr<bool>> Handle(EditUserCommand command, CancellationToken cancellationToken)
    {
        if (await _userQueryRepository.GetUserByIdentity(
                command.Username,
                command.Email,
                command.Phone)
            is User existingUser && existingUser.Id != command.UserId)
        {
            List<Error> errors = new ();
            
            if (existingUser.Username == command.Username) 
                errors.Add(Errors.User.UniqueUsername);
            
            if (existingUser.Email == command.Email) 
                errors.Add(Errors.User.UniqueEmail);
            
            if (existingUser.Phone == command.Phone) 
                errors.Add(Errors.User.UniquePhone);
            
            return errors;
        }

        string? imagePath = null;
        if (command.Image is not null)
            imagePath = await _fileDownloader.Download(command.Image, "profiles");
        
        
        var user = await _userQueryRepository.GetUserById(command.UserId);
        if (user is null)
            return Errors.User.UserNotFound;


        var newUser = User.Create(
            command.Username,
            command.Fullname,
            command.Email,
            command.Phone,
            user.Password,
            user.Profile
        );
        newUser.SetId(command.UserId);

        var newProfile = UserProfile.Create(imagePath, command.Bio);
        newProfile.SetId(user.Profile.Id);
        newUser.SetProfile(newProfile);
        
        try
        {
            await _userCommandRepository.UpdateUser(newUser);
            return true;
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }
        
    }
}