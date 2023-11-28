﻿using Instagram.Application.Services.UserService.Commands;
using Instagram.Application.Services.UserService.Queries.GetAllUsers;
using Instagram.Application.Services.UserService.Queries.GetUser;
using Instagram.Contracts.User;
using Instagram.Contracts.User.EditUserContracts;
using Instagram.Contracts.User.GetAllUsersContracts;
using Instagram.Contracts.User.GetUserContracts;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Infrastructure.Services;

using Mapster;

namespace Instagram.WebApi.Common.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetUserRequest, GetUserQuery>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<GetUserResult, GetUserResponse>()
            .Map(dest => dest.Profile, src => src.User.Profile)
            .Map(dest => dest, src => src.User);

        
        config.NewConfig<(long userId, EditUserRequest request), EditUserCommand>()
            .Map(dest => dest.Image , src => src.request.Image != null ? new AppFileProxy(src.request.Image) : null)
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
        

        config.NewConfig<GetAllUsersRequest, GetAllUsersQuery>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<User, GetAllUser>()
            .Map(dest => dest.Image, src => src.Profile.Image)
            .Map(dest => dest, src => src);
        
        config.NewConfig<GetAllUsersResult, GetAllUsersResponse>()
            .Map(dest => dest.Users, src => src.Users);

        

    }
}