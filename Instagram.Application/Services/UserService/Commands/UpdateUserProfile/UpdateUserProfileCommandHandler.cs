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
    private readonly IFileDownloader _fileDownloader;
    private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

    public UpdateUserProfileCommandHandler(
        IDapperUserRepository dapperUserRepository,
        IEfUserRepository efUserRepository,
        IFileDownloader fileDownloader,
        ILogger<UpdateUserProfileCommandHandler> logger)
    {
        _dapperUserRepository = dapperUserRepository;
        _efUserRepository = efUserRepository;
        _fileDownloader = fileDownloader;
        _logger = logger;
    }

    public async Task<ErrorOr<bool>> Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var profile = await _dapperUserRepository.GetUserProfile(command.UserId);
            if (profile is null)
                return Errors.Common.NotFound;
            
            string? imagePath = null;
            try
            {
                if (command.Image is not null)
                    imagePath = await _fileDownloader.Download(command.Image, "profiles");
            }
            catch (FileSaveException)
            {
                return Errors.File.DownloadFailed;
            }
            
            UserGender? gender = null;
            if (command.Gender != profile.Gender?.Name && command.Gender != null)
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
                Id = profile.Id,
                Fullname = command.Fullname,
                UserId = profile.UserId,
                Image = imagePath,
                Bio = command.Bio,
                Gender = gender
            };
            
            if (updatedProfile.Different(profile))
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