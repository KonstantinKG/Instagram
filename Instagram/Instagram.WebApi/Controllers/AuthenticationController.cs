using Instagram.Application.Services.Authentication;
using Instagram.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.WebApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login(LoginRequest request)
    {
        var serviceResult = _authenticationService.Login(request.Email, request.Password);
        var response = new AuthenticationResponse(
            serviceResult.User.Id,
            serviceResult.User.Name,
            serviceResult.User.Email,
            serviceResult.Token
        );
        return Ok(response);
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var serviceResult = _authenticationService.Register(request.Name, request.Email, request.Password);

        var response = new AuthenticationResponse(
            serviceResult.User.Id,
            serviceResult.User.Name,
            serviceResult.User.Email,
            serviceResult.Token
        );

        return Ok(response);
    }
}