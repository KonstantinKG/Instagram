using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Instagram.Application.Services.PostService.Queries.AllHomePosts;

public class AllHomePostsQueryHandler
{
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ApplicationSettings _applicationSettings;
    private readonly ILogger<AllHomePostsQueryHandler> _logger;

    public AllHomePostsQueryHandler(
        IDapperPostRepository dapperUserRepository,
        IOptions<ApplicationSettings> applicationOptions,
        ILogger<AllHomePostsQueryHandler> logger)
    {
        _dapperPostRepository = dapperUserRepository;
        _logger = logger;
        _applicationSettings = applicationOptions.Value;
    }
    
    public async Task<ErrorOr<AllHomePostsResult>> Handle(AllHomePostsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var limit = _applicationSettings.PaginationLimit;
            var offset = (query.Page - 1) *  limit;
            var total = await _dapperPostRepository.GetTotalHomePosts(query.Date);
            var pages = total /  limit + (total %  limit > 0 ? 1 : 0);

            var posts = new List<Post>();
            if (query.Page <= total)
                posts = await _dapperPostRepository.AllHomePosts(query.UserId, offset,  limit, query.Date);
            
            return new AllHomePostsResult(
                query.Page,
                pages,
                posts
            );  
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }
    }
}