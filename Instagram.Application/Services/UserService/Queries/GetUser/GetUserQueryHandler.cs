using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Application.Common.Interfaces.Persistence.QueryRepositories;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.UserService.Queries.GetUser;

public class GetUserQueryHandler
{
    private readonly IUserQueryRepository _userQueryRepository;

    public GetUserQueryHandler(IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }
    
    public async Task<ErrorOr<GetUserResult>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        if (await _userQueryRepository.GetUserById(query.UserId) is not User user)
        {
            return Errors.User.UserNotFound;
        }

        return new GetUserResult(user);
    }
}