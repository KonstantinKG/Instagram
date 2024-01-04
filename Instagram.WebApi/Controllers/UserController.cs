using ErrorOr;

using Instagram.Application.Services.UserService.Commands.UpdateUser;
using Instagram.Application.Services.UserService.Queries.GetAllUsers;
using Instagram.Application.Services.UserService.Queries.GetUser;
using Instagram.Contracts.User.GetAllUsersContracts;
using Instagram.Contracts.User.GetUserContracts;
using Instagram.Contracts.User.UpdateUserContracts;

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
    [Route("get")]
    public async Task<IActionResult> Get([FromQuery] GetUserRequest request)
    {
        var userId = request.Id ?? GetUserId();
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

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Edit([FromForm] UpdateUserRequest request)
    {
        var userId = GetUserId();
        var editUserCommand = _mapper.Map<UpdateUserCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<UpdateUserCommandHandler>();
        ErrorOr<bool> serviceResult = await handler.Handle(editUserCommand, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
}