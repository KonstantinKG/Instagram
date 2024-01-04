using Instagram.Application.Services.UserService.Commands;
using Instagram.Application.Services.UserService.Commands.UpdateUser;
using Instagram.Application.Services.UserService.Queries.GetAllUsers;
using Instagram.Application.Services.UserService.Queries.GetUser;
using Instagram.Contracts.User.GetAllUsersContracts;
using Instagram.Contracts.User.GetUserContracts;
using Instagram.Contracts.User.UpdateUserContracts;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Aggregates.UserAggregate.Entities;
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
            .Map(dest => dest.Profile.Gender, src => src.User.Profile.Gender != null ? src.User.Profile.Gender.Name : null)
            .Map(dest => dest, src => src.User);

        
        config.NewConfig<(Guid userId, UpdateUserRequest request), UpdateUserCommand>()
            .Map(dest => dest.Image , src => src.request.Image != null ? new AppFileProxy(src.request.Image) : null)
            .Map(dest => dest.Id, src => src.userId)
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