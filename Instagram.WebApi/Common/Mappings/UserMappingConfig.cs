using Instagram.Application.Services.UserService.Commands.SubscribeUser;
using Instagram.Application.Services.UserService.Commands.UnsubscribeUser;
using Instagram.Application.Services.UserService.Commands.UpdateUserProfile;
using Instagram.Application.Services.UserService.Queries.AllUserSubscriptions;
using Instagram.Application.Services.UserService.Queries.GetAllUsers;
using Instagram.Application.Services.UserService.Queries.GetUser;
using Instagram.Contracts.User._Common;
using Instagram.Contracts.User.AllUserSubscriptions;
using Instagram.Contracts.User.GetAllUsersContracts;
using Instagram.Contracts.User.GetUserContracts;
using Instagram.Contracts.User.SubscribeUserContracts;
using Instagram.Contracts.User.UnsubscribeUserContracts;
using Instagram.Contracts.User.UpdateUserProfileContracts;
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

        config.NewConfig<GetUserResult, UserResponse>()
            .Map(dest => dest.Profile, src => src.User.Profile)
            .Map(dest => dest.Profile.Gender, src => src.User.Profile.Gender != null ? src.User.Profile.Gender.Name : null)
            .Map(dest => dest, src => src.User);

        
        config.NewConfig<(Guid userId, UpdateUserProfileRequest request), UpdateUserProfileCommand>()
            .Map(dest => dest.Image , src => src.request.Image != null ? new AppFileProxy(src.request.Image) : null)
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<GetAllUsersRequest, GetAllUsersQuery>()
            .Map(dest => dest, src => src);
        
        config.NewConfig<User, UserShortResponse>()
            .Map(dest => dest.Image, src => src.Profile.Image)
            .Map(dest => dest, src => src);
        
        config.NewConfig<GetAllUsersResult, GetAllUsersResponse>()
            .Map(dest => dest.Users, src => src.Users)
            .Map(dest => dest, src => src);
        
        config.NewConfig<(Guid subscriberId, SubscribeUserRequest request), SubscribeUserCommand>()
            .Map(dest => dest.SubscriberId, src => src.subscriberId)
            .Map(dest => dest, src => src.request);

        config.NewConfig<(Guid subscriberId, UnsubscribeUserRequest request), UnsubscribeUserCommand>()
            .Map(dest => dest.SubscriberId, src => src.subscriberId)
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<(Guid subscriberId, AllUserSubscriptionsRequest request), AllUserSubscriptionsQuery>()
            .Map(dest => dest.SubscriberId, src => src.subscriberId)
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<AllUserSubscriptionsResult, AllUserSubscriptionsResponse>()
            .Map(dest => dest.Subscriptions, src => src.Subscriptions)
            .Map(dest => dest, src => src);
    }
}