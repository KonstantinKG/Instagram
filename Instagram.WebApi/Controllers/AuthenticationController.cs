using ErrorOr;

using Instagram.Application.Services.Authentication.Commands.Logout;
using Instagram.Application.Services.Authentication.Commands.Refresh;
using Instagram.Application.Services.Authentication.Commands.Register;
using Instagram.Application.Services.Authentication.Common;
using Instagram.Application.Services.Authentication.Queries.Login;
using Instagram.Contracts.Authentication;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var loginQuery = _mapper.Map<LoginQuery>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<LoginQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<LoginQueryPipeline>();
        ErrorOr<AuthenticationResult> serviceResult = await pipeline.Pipe(loginQuery, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AuthenticationResponse>(result)),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    [AllowAnonymous]
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
    
    [HttpPost]
    [AllowAnonymous]
    [Route("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        var refreshCommand = _mapper.Map<RefreshCommand>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<RefreshCommandHandler>();
        ErrorOr<AuthenticationResult> serviceResult = await handler.Handle(refreshCommand, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AuthenticationResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    [Authorize]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        var sessionId = HttpContext.User.Claims.First(c => c.Type == "sid").Value;
        
        var command = new LogoutCommand(sessionId);
        var handler = HttpContext.RequestServices.GetRequiredService<LogoutCommandHandler>();
        ErrorOr<bool> serviceResult = await handler.Handle(command, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(),
            errors => Problem(errors)
        );
    }
}