using ErrorOr;
using Instagram.Application.Services.Authentication;
using Instagram.Contracts.Authentication;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.WebApi.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;

    public AuthenticationController(IAuthenticationService authenticationService, IMapper mapper)
    {
        _authenticationService = authenticationService;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login(LoginRequest request)
    {
        var loginCommand = _mapper.Map<LoginCommand>(request);
        ErrorOr<AuthenticationResult> serviceResult = _authenticationService.Login(loginCommand);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AuthenticationResponse>(result)),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var registerCommand = _mapper.Map<RegisterCommand>(request);
        ErrorOr<AuthenticationResult> serviceResult = _authenticationService.Register(registerCommand);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AuthenticationResponse>(result)),
            errors => Problem(errors)
        );
    }
}