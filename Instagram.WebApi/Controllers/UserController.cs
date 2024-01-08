using ErrorOr;

using Instagram.Application.Services.UserService.Commands.SubscribeUser;
using Instagram.Application.Services.UserService.Commands.UnsubscribeUser;
using Instagram.Application.Services.UserService.Commands.UpdateUserProfile;
using Instagram.Application.Services.UserService.Queries.AllUserSubscriptions;
using Instagram.Application.Services.UserService.Queries.GetAllUsers;
using Instagram.Application.Services.UserService.Queries.GetUser;
using Instagram.Contracts.User._Common;
using Instagram.Contracts.User.AllUserSubscriptions;
using Instagram.Contracts.User.GetAllUsersContracts;
using Instagram.Contracts.User.GetUserContracts;
using Instagram.Contracts.User.SubscribeUserContracts;
using Instagram.Contracts.User.UnsubscribeUserContracts;
using Instagram.Contracts.User.UpdateUserProfileContracts;

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
        var query = new GetUserQuery(userId);
        var handler = HttpContext.RequestServices.GetRequiredService<GetUserQueryHandler>();
        ErrorOr<GetUserResult> serviceResult = await handler.Handle(query, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<UserResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> All([FromQuery] GetAllUsersRequest request)
    {
        var query = _mapper.Map<GetAllUsersQuery>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<GetAllUsersQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<GetAllUsersQueryPipeline>();
        ErrorOr<GetAllUsersResult> serviceResult = await pipeline.Pipe(query, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<GetAllUsersResponse>(result)),
            errors => Problem(errors)
        );
    }

        
    [HttpPost]
    [Route("subscribe")]
    public async Task<IActionResult> Subscribe(SubscribeUserRequest request)
    {
        var userId = GetUserId();
        var command = _mapper.Map<SubscribeUserCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<SubscribeUserCommandHandler>();
        ErrorOr<SubscribeUserResult> serviceResult = await handler.Handle(command, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    [Route("unsubscribe")]
    public async Task<IActionResult> Unsubscribe(UnsubscribeUserRequest request)
    {
        var userId = GetUserId();
        var command = _mapper.Map<UnsubscribeUserCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<UnsubscribeUserCommandHandler>();
        ErrorOr<UnsubscribeUserResult> serviceResult = await handler.Handle(command, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpPut]
    [Route("profile/update")]
    public async Task<IActionResult> Edit([FromForm] UpdateUserProfileRequest profileRequest)
    {
        var userId = GetUserId();
        var command = _mapper.Map<UpdateUserProfileCommand>((userId, profileRequest));
        var handler = HttpContext.RequestServices.GetRequiredService<UpdateUserProfileCommandHandler>();
        ErrorOr<bool> serviceResult = await handler.Handle(command, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }

    
    [HttpGet]
    [Route("subscriptions/all")]
    public async Task<IActionResult> AllSubscriptions([FromQuery] AllUserSubscriptionsRequest request)
    {
        var userId = GetUserId();
        var query = _mapper.Map<AllUserSubscriptionsQuery>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<AllUserSubscriptionsQueryHandler>();
        ErrorOr<AllUserSubscriptionsResult> serviceResult = await handler.Handle(query, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AllUserSubscriptionsResponse>(result)),
            errors => Problem(errors)
        );
    }
}