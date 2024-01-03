using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Instagram.Application.Services.PostService.Queries.AllPosts;

public class AllPostsQueryHandler
{
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ApplicationSettings _applicationSettings;
    private readonly ILogger<AllPostsQueryHandler> _logger;

    public AllPostsQueryHandler(
        IDapperPostRepository dapperUserRepository,
        IOptions<ApplicationSettings> applicationOptions,
        ILogger<AllPostsQueryHandler> logger)
    {
        _dapperPostRepository = dapperUserRepository;
        _logger = logger;
        _applicationSettings = applicationOptions.Value;
    }
    
    public async Task<ErrorOr<AllPostsResult>> Handle(AllPostsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var limit = _applicationSettings.PaginationLimit;
            var offset = (query.Page - 1) *  limit;
            var total = await _dapperPostRepository.GetTotalPosts();
            var pages = total /  limit + (total %  limit > 0 ? 1 : 0);

            var posts = new List<Post>();
            if (query.Page <= total)
                posts = await _dapperPostRepository.AllPosts(offset,  limit, query.Date);
            
            return new AllPostsResult(
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