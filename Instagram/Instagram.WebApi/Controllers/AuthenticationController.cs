using ErrorOr;
using Instagram.Application.Services.Authentication.Commands.Register;
using Instagram.Application.Services.Authentication.Common;
using Instagram.Application.Services.Authentication.Queries.Login;
using Instagram.Contracts.Authentication;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.WebApi.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly IMapper _mapper;

    public AuthenticationController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var loginQuery = _mapper.Map<LoginQuery>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<LoginQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<LoginQueryPipeline>();
        ErrorOr<AuthenticationResult> serviceResult = await pipeline.Pipe(loginQuery, handler.Handle);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AuthenticationResponse>(result)),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var registerCommand = _mapper.Map<RegisterCommand>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<RegisterCommandHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<RegisterCommandPipeline>();
        ErrorOr<AuthenticationResult> serviceResult = await pipeline.Pipe(registerCommand, handler.Handle);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AuthenticationResponse>(result)),
            errors => Problem(errors)
        );
    }
}