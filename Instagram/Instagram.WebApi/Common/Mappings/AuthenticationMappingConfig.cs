using Instagram.Application.Services.Authentication;
using Instagram.Contracts.Authentication;
using Mapster;

namespace Instagram.WebApi.Common.Mappings;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Token, src => src.Token)
            .Map(dest => dest, src => src.User);
    }
}