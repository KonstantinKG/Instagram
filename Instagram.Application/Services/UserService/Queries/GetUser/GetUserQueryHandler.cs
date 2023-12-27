using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.UserService.Queries.GetUser;

public class GetUserQueryHandler
{
    private readonly IDapperUserRepository _dapperUserRepository;

    public GetUserQueryHandler(IDapperUserRepository dapperUserRepository)
    {
        _dapperUserRepository = dapperUserRepository;
    }
    
    public async Task<ErrorOr<GetUserResult>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        if (await _dapperUserRepository.GetUserById(query.Id) is not User user)
        {
            return Errors.Common.NotFound;
        }

        return new GetUserResult(user);
    }
}