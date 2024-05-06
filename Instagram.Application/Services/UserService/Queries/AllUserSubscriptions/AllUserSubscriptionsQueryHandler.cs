using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Configurations;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Instagram.Application.Services.UserService.Queries.AllUserSubscriptions;

public class AllUserSubscriptionsQueryHandler
{
    private readonly AppConfiguration _configuration;
    private readonly IDapperUserRepository _dapperUserRepository;
    private readonly ILogger<AllUserSubscriptionsQueryHandler> _logger;

    public AllUserSubscriptionsQueryHandler(
        IOptions<AppConfiguration> options,
        IDapperUserRepository dapperUserRepository,
        ILogger<AllUserSubscriptionsQueryHandler> logger)
    {
        _configuration = options.Value;
        _dapperUserRepository = dapperUserRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<AllResult<User>>> Handle(AllUserSubscriptionsQuery query,  CancellationToken cancellationToken)
    {
        try
        {
            var limit = _configuration.Application.PaginationLimit;
            var offset = (query.Page - 1) *  limit;
            var total = await _dapperUserRepository.GetTotalUserSubscriptions(query.SubscriberId);
            var pages = total /  limit + (total %  limit > 0 ? 1 : 0);

            var subscriptions = new List<User>();
            if (query.Page <= total)
                subscriptions = await _dapperUserRepository.GetAllUserSubscriptions(query.SubscriberId, offset,  limit);
            
            return new AllResult<User>(
                query.Page,
                pages,
                total,
                subscriptions
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }
    }
}