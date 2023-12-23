using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Common.Exceptions;

namespace Instagram.Application.Services.UserService.Commands.EditUser;

public class EditUserCommandHandler
{
    private readonly IDapperUserRepository _dapperUserRepository;
    private readonly IEfUserRepository _efUserRepository;
    private readonly IFileDownloader _fileDownloader;
    
    public EditUserCommandHandler(
        IDapperUserRepository dapperUserRepository,
        IEfUserRepository efUserRepository,
        IFileDownloader fileDownloader)
    {
        _dapperUserRepository = dapperUserRepository;
        _efUserRepository = efUserRepository;
        _fileDownloader = fileDownloader;
    }

    public async Task<ErrorOr<bool>> Handle(EditUserCommand command, CancellationToken cancellationToken)
    {
        if (await _dapperUserRepository.GetUserByIdentity(
                command.Username,
                command.Email,
                command.Phone)
            is User existingUser && existingUser.Id.Value.ToString() != command.UserId)
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
        try
        {
            if (command.Image is not null)
                imagePath = await _fileDownloader.Download(command.Image, "profiles");
        }
        catch (FileDownloadException)
        {
            return Errors.File.DownloadFailed;
        }
        
        var user = await _dapperUserRepository.GetUserById(command.UserId);
        if (user is null)
            return Errors.User.UserNotFound;

        var updatedProfile = UserProfile.Fill(
            user.Profile.Id,
            user.Id,
            imagePath,
            command.Bio,
            null
        );

        var updatedUser = User.Fill(
            user.Id,
            command.Username,
            command.Fullname,
            command.Email,
            command.Phone,
            user.Password,
            updatedProfile
        );
        
        try
        {
            await _efUserRepository.UpdateUser(updatedUser);
            return true;
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }
        
    }
}