using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Services.PostService.Queries._Common;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Configurations;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Instagram.Application.Services.PostService.Queries.AllUserPosts;

public class AllUserPostsQueryHandler
{
    private readonly AppConfiguration _configuration;
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ILogger<AllUserPostsQueryHandler> _logger;

    public AllUserPostsQueryHandler(
        IOptions<AppConfiguration> options,
        IDapperPostRepository dapperUserRepository,
        ILogger<AllUserPostsQueryHandler> logger)
    {
        _configuration = options.Value;
        _dapperPostRepository = dapperUserRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<AllResult<Post>>> Handle(AllUserPostsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var limit = _configuration.Application.PaginationLimit;
            var offset = (query.Page - 1) *  limit;
            var total = await _dapperPostRepository.GetTotalUserPosts(query.UserId, query.Date);
            var pages = total /  limit + (total %  limit > 0 ? 1 : 0);

            var posts = new List<Post>();
            if (query.Page <= total)
                posts = await _dapperPostRepository.AllUserPosts(query.UserId, offset,  limit, query.Date);
            
            return new AllResult<Post>(
                query.Page,
                pages,
                total,
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