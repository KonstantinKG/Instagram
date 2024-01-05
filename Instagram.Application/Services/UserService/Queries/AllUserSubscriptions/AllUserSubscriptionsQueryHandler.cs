using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.UserAggregate;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Instagram.Application.Services.UserService.Queries.AllUserSubscriptions;

public class AllUserSubscriptionsQueryHandler
{
    private readonly IDapperUserRepository _dapperUserRepository;
    private readonly ApplicationSettings _applicationSettings;
    private readonly ILogger<AllUserSubscriptionsQueryHandler> _logger;

    public AllUserSubscriptionsQueryHandler(
        IDapperUserRepository dapperUserRepository,
        IOptions<ApplicationSettings> applicationOptions,
        ILogger<AllUserSubscriptionsQueryHandler> logger)
    {
        _dapperUserRepository = dapperUserRepository;
        _applicationSettings = applicationOptions.Value;
        _logger = logger;
    }
    
    public async Task<ErrorOr<AllUserSubscriptionsResult>> Handle(AllUserSubscriptionsQuery query,  CancellationToken cancellationToken)
    {
        try
        {
            var limit = _applicationSettings.PaginationLimit;
            var offset = (query.Page - 1) *  limit;
            var total = await _dapperUserRepository.GetTotalUserSubscriptions(query.SubscriberId);
            var pages = total /  limit + (total %  limit > 0 ? 1 : 0);

            var subscriptions = new List<User>();
            if (query.Page <= total)
                subscriptions = await _dapperUserRepository.GetAllUserSubscriptions(query.SubscriberId, offset,  limit);
            
            return new AllUserSubscriptionsResult(
                query.Page,
                pages,
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