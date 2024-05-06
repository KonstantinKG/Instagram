using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Configurations;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Instagram.Application.Services.UserService.Queries.GetAllUsers;

public class GetAllUsersQueryHandler
{
    private readonly AppConfiguration _configuration;
    private readonly IDapperUserRepository _dapperUserRepository;
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    public GetAllUsersQueryHandler(
        IOptions<AppConfiguration> options,
        IDapperUserRepository dapperUserRepository,
        ILogger<GetAllUsersQueryHandler> logger)
    {
        _configuration = options.Value;
        _dapperUserRepository = dapperUserRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<AllResult<User>>> Handle(GetAllUsersQuery query,  CancellationToken cancellationToken)
    {
        try
        {
            var limit = _configuration.Application.PaginationLimit;
            var offset = (query.Page - 1) *  limit;
            var total = await _dapperUserRepository.GetTotalUsers();
            var pages = total /  limit + (total %  limit > 0 ? 1 : 0);

            var users = new List<User>();
            if (query.Page <= total)
                users = await _dapperUserRepository.GetAllUsers(offset,  limit);
            
            return new AllResult<User>(
                query.Page,
                pages,
                total,
                users
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }
    }
}