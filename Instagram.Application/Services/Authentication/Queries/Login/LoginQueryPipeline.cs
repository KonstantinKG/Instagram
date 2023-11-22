using Instagram.Application.Common.Interfaces.Pipeline;
using Instagram.Application.Services.Authentication.Common;

namespace Instagram.Application.Services.Authentication.Queries.Login;

public class LoginQueryPipeline : BasePipeline<LoginQuery, AuthenticationResult>
{
    public LoginQueryPipeline(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}