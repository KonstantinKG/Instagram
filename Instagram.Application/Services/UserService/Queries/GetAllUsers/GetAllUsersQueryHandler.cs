using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.QueryRepositories;
using Instagram.Application.Services.UserService.Queries.GetUser;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.UserService.Queries.GetAllUsers;

public class GetAllUsersQueryHandler
{
    private readonly IUserQueryRepository _userQueryRepository;

    public GetAllUsersQueryHandler(IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }
    
    public async Task<ErrorOr<GetAllUsersResult>> Handle(CancellationToken cancellationToken)
    {
        try
        {
            var users = await _userQueryRepository.GetAllUsers();
            return new GetAllUsersResult(users);
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }

    }
}