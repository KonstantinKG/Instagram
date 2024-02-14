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
using Instagram.Domain.Aggregates.UserAggregate.Entities;
using Instagram.Infrastructure.Services;

using Mapster;

namespace Instagram.WebApi.Common.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserResponse>()
            .Map(dest => dest.id, src => src.Id)
            .Map(dest => dest.username, src => src.Username)
            .Map(dest => dest.email, src => src.Email)
            .Map(dest => dest.phone, src => src.Phone)
            .Map(dest => dest.profile, src => src.Profile);

        config.NewConfig<UserProfile, UserProfileResponse>()
            .Map(dest => dest.image, src => src.Image)
            .Map(dest => dest.bio, src => src.Bio)
            .Map(dest => dest.full_name, src => src.Fullname)
            .Map(dest => dest.gender, src => src.Gender);
        
        config.NewConfig<GetUserRequest, GetUserQuery>()
            .Map(dest => dest.Id, src => src.id);
        
        config.NewConfig<GetAllUsersRequest, GetAllUsersQuery>()
            .Map(dest => dest.Page, src => src.page);
        
        config.NewConfig<(Guid userId, UpdateUserProfileRequest request), UpdateUserProfileCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.Fullname, src => src.request.fullname)
            .Map(dest => dest.Image , src => src.request.image != null ? new AppFileProxy(src.request.image) : null)
            .Map(dest => dest.Bio, src => src.request.bio)
            .Map(dest => dest.Gender, src => src.request.gender);
        
        config.NewConfig<GetAllUsersResult, GetAllUsersResponse>()
            .Map(dest => dest.current, src => src.Current)
            .Map(dest => dest.total, src => src.Total)
            .Map(dest => dest.users, src => src.Users);

        config.NewConfig<(Guid subscriberId, SubscribeUserRequest request), SubscribeUserCommand>()
            .Map(dest => dest.UserId, src => src.request.user_id)
            .Map(dest => dest.SubscriberId, src => src.subscriberId);

        config.NewConfig<(Guid subscriberId, UnsubscribeUserRequest request), UnsubscribeUserCommand>()
            .Map(dest => dest.UserId, src => src.request.user_id)
            .Map(dest => dest.SubscriberId, src => src.subscriberId);
        
        config.NewConfig<(Guid subscriberId, AllUserSubscriptionsRequest request), AllUserSubscriptionsQuery>()
            .Map(dest => dest.Page, src => src.request.page)
            .Map(dest => dest.SubscriberId, src => src.subscriberId);
        
        config.NewConfig<AllUserSubscriptionsResult, AllUserSubscriptionsResponse>()
            .Map(dest => dest.current, src => src.Current)
            .Map(dest => dest.total, src => src.Total)
            .Map(dest => dest.subscriptions, src => src.Subscriptions);
    }
}