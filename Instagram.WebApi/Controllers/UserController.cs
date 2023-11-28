using ErrorOr;

using Instagram.Application.Services.UserService.Commands;
using Instagram.Application.Services.UserService.Queries.GetAllUsers;
using Instagram.Application.Services.UserService.Queries.GetUser;
using Instagram.Contracts.User.EditUserContracts;
using Instagram.Contracts.User.GetAllUsersContracts;
using Instagram.Contracts.User.GetUserContracts;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc;

namespace Instagram.WebApi.Controllers;

[Route("user")]
public class UserController : ApiController
{
    private readonly IMapper _mapper;

    public UserController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet]
    [Route("get/")]
    public async Task<IActionResult> Get([FromQuery] GetUserRequest request)
    {
        var requestUserId = request.UserId;
        var claimsUserIdText = HttpContext.User.Claims.First(x => x.Type == "nameid").Value;
        var claimsUserId = long.Parse(claimsUserIdText); 

        var userId = requestUserId ?? claimsUserId;
        var getUserQuery = new GetUserQuery(userId);
        var handler = HttpContext.RequestServices.GetRequiredService<GetUserQueryHandler>();
        ErrorOr<GetUserResult> serviceResult = await handler.Handle(getUserQuery, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<GetUserResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> All([FromQuery] GetAllUsersRequest request)
    {
        var getAllUsersQuery = _mapper.Map<GetAllUsersQuery>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<GetAllUsersQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<GetAllUsersQueryPipeline>();
        ErrorOr<GetAllUsersResult> serviceResult = await pipeline.Pipe(getAllUsersQuery, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<GetAllUsersResponse>(result)),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    [Route("edit")]
    public async Task<IActionResult> Edit([FromForm] EditUserRequest request)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "nameid").Value;
        var editUserCommand = _mapper.Map<EditUserCommand>((long.Parse(userId), request));
        var handler = HttpContext.RequestServices.GetRequiredService<EditUserCommandHandler>();
        ErrorOr<bool> serviceResult = await handler.Handle(editUserCommand, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
}