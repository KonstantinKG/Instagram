using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Common.Exceptions;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.UserService.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler
{
    private readonly IDapperUserRepository _dapperUserRepository;
    private readonly IEfUserRepository _efUserRepository;
    private readonly FileProvider _fileProvider;
    private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

    public UpdateUserProfileCommandHandler(
        IDapperUserRepository dapperUserRepository,
        IEfUserRepository efUserRepository,
        ILogger<UpdateUserProfileCommandHandler> logger,
        FileProvider fileProvider)
    {
        _dapperUserRepository = dapperUserRepository;
        _efUserRepository = efUserRepository;
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<ErrorOr<bool>> Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var profile = await _dapperUserRepository.GetUserProfile(command.UserId);
            
            string? imagePath = null;
            try
            {
                if (command.Image is not null)
                    imagePath = await _fileProvider.Save(command.Image);
            }
            catch (FileSaveException)
            {
                return Errors.File.DownloadFailed;
            }
            
            UserGender? gender = null;
            if (command.Gender != profile?.Gender?.Name && command.Gender != null)
            {
                if (await _dapperUserRepository.GetUserGender(command.Gender) is UserGender userGender)
                {
                    gender = userGender;
                }
                else
                {
                    gender = new UserGender { Name = command.Gender };
                    await _efUserRepository.AddUserGender(gender);
                }
            }
            
            var updatedProfile = new UserProfile {
                Id = profile != null ? profile.Id : Guid.NewGuid(),
                UserId = command.UserId,
                Fullname = command.Fullname,
                Image = imagePath,
                Bio = command.Bio,
                Gender = gender
            };
            
            if (profile == null || updatedProfile.Different(profile))
                await _efUserRepository.UpdateUserProfile(updatedProfile);
            
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }
    }
}