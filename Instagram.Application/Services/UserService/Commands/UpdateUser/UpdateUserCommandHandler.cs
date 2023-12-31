﻿using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Common.Exceptions;

namespace Instagram.Application.Services.UserService.Commands.UpdateUser;

public class UpdateUserCommandHandler
{
    private readonly IDapperUserRepository _dapperUserRepository;
    private readonly IEfUserRepository _efUserRepository;
    private readonly IFileDownloader _fileDownloader;
    
    public UpdateUserCommandHandler(
        IDapperUserRepository dapperUserRepository,
        IEfUserRepository efUserRepository,
        IFileDownloader fileDownloader)
    {
        _dapperUserRepository = dapperUserRepository;
        _efUserRepository = efUserRepository;
        _fileDownloader = fileDownloader;
    }

    public async Task<ErrorOr<bool>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (await _dapperUserRepository.GetUserByIdentity(
                command.Username,
                command.Email,
                command.Phone)
            is User existingUser && existingUser.Id != command.Id)
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
        catch (FileSaveException)
        {
            return Errors.File.DownloadFailed;
        }
        
        var user = await _dapperUserRepository.GetUserById(command.Id);
        if (user is null)
            return Errors.Common.NotFound;
        
        UserGender? gender = null;
        if (command.Gender != user.Profile.Gender?.Name)
        {
            if (command.Gender != null)
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
            
        }

        UserProfile? updatedProfile = null;
        var tempProfile = new UserProfile {
            Id = user.Profile.Id,
            UserId = user.Id,
            Image = imagePath,
            Bio = command.Bio,
            Gender = gender
        };
        
        if (user.Profile.Different(tempProfile))
        {
            updatedProfile = tempProfile;
        }

        User? updatedUser = null;
        var tempUser = new User { 
            Id = user.Id,
            Username = command.Username,
            Fullname = command.Fullname,
            Email = command.Email, 
            Phone = command.Phone,
            Password = user.Password,
            Profile = updatedProfile ?? user.Profile
        };

        if (user.Different(tempUser))
        {
            updatedUser = tempUser;
        }
        
        try
        {
            await _efUserRepository.UpdateUser(updatedUser, updatedProfile);
            return true;
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }

    }
}