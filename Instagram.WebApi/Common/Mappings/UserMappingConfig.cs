using Instagram.Application.Common.Interfaces.Services;
using Instagram.Application.Services.UserService.Commands;
using Instagram.Application.Services.UserService.Queries.GetUser;
using Instagram.Contracts.User;
using Instagram.Infrastructure.Services;

using Mapster;

namespace Instagram.WebApi.Common.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetUserResult, GetUserResponse>()
            .Map(dest => dest.Profile, src => src.User.Profile)
            .Map(dest => dest, src => src.User);

        config.NewConfig<(string userId, EditUserRequest request), EditUserCommand>()
            .Map(dest => dest.Image , src => src.request.Image != null ? new AppFileProxy(src.request.Image) : null)
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
    }
}