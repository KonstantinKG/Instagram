using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Instagram.Application.Services.UserService.Queries.GetAllUsers;

public class GetAllUsersQueryHandler
{
    private readonly IDapperUserRepository _dapperUserRepository;
    private readonly ApplicationSettings _applicationSettings;
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    public GetAllUsersQueryHandler(
        IDapperUserRepository dapperUserRepository,
        IOptions<ApplicationSettings> applicationOptions,
        ILogger<GetAllUsersQueryHandler> logger)
    {
        _dapperUserRepository = dapperUserRepository;
        _logger = logger;
        _applicationSettings = applicationOptions.Value;
    }
    
    public async Task<ErrorOr<GetAllUsersResult>> Handle(GetAllUsersQuery query,  CancellationToken cancellationToken)
    {
        try
        {
            var limit = _applicationSettings.PaginationLimit;
            var offset = (query.Page - 1) *  limit;
            var total = await _dapperUserRepository.GetTotalUsers();
            var pages = total /  limit + (total %  limit > 0 ? 1 : 0);

            var users = new List<User>();
            if (query.Page <= total)
                users = await _dapperUserRepository.GetAllUsers(offset,  limit);
            
            return new GetAllUsersResult(
                query.Page,
                pages,
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