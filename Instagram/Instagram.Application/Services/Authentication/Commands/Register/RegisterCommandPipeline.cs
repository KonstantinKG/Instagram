using Instagram.Application.Common.Interfaces.Pipeline;
using Instagram.Application.Services.Authentication.Common;

namespace Instagram.Application.Services.Authentication.Commands.Register;

public class RegisterCommandPipeline : BasePipeline<RegisterCommand, AuthenticationResult>
{
    public RegisterCommandPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}