using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.UserService.Queries.GetAllUsers;

public class GetAllUsersQueryHandler
{
    private readonly IDapperUserRepository _dapperUserRepository;

    public GetAllUsersQueryHandler(IDapperUserRepository dapperUserRepository)
    {
        _dapperUserRepository = dapperUserRepository;
    }
    
    public async Task<ErrorOr<GetAllUsersResult>> Handle(GetAllUsersQuery query,  CancellationToken cancellationToken)
    {
        try
        {
            const int limit = 15;
            var offset = (query.Page - 1) * limit;
            var total = await _dapperUserRepository.GetTotalUsers();
            var pages = total / limit + (total % limit > 0 ? 1 : 0);

            var users = new List<User>();
            if (query.Page <= total)
                users = await _dapperUserRepository.GetAllUsers(offset, limit);
            
            return new GetAllUsersResult(
                query.Page,
                pages,
                users
            );
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }

    }
}