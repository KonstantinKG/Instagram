using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Instagram.Application.Services.PostService.Queries.GetHomePostsSlider;

public class GetHomePostsSliderQueryHandler
{
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ApplicationSettings _applicationSettings;
    private readonly ILogger<GetHomePostsSliderQueryHandler> _logger;

    public GetHomePostsSliderQueryHandler(
        IDapperPostRepository dapperUserRepository,
        IOptions<ApplicationSettings> applicationOptions, 
        ILogger<GetHomePostsSliderQueryHandler> logger)
    {
        _dapperPostRepository = dapperUserRepository;
        _logger = logger;
        _applicationSettings = applicationOptions.Value;
    }
    
    public async Task<ErrorOr<GetHomePostsSliderResult>> Handle(GetHomePostsSliderQuery query,  CancellationToken cancellationToken)
    {
        try
        {
            var limit = _applicationSettings.PaginationLimit;
            var offset = (query.Page - 1) * limit;
            int? previousOffset = query.Page == 1 ? null : offset - limit;
            var nextOffset = offset + limit;

            var posts =  await _dapperPostRepository.AllHomePosts(query.UserId, offset,  limit, query.Date);

            Guid? previousPostId = null;
            Guid? nextPostId = null;
            Post? currentPost = null;
            
            for (int i = 0; i < posts.Count; i++)
            {
                var post = posts[i];
                if (post.Id != query.PostId)
                    continue;
                
                if (i == 0)
                {
                    var previousPosts = previousOffset != null ? await _dapperPostRepository.AllHomePosts(query.UserId, (int)previousOffset, limit, query.Date) : null;
                    previousPostId = previousPosts?.Last().Id;    
                    nextPostId = posts.Count > 1 ? posts[1].Id : null;
                }
                else if (i == posts.Count)
                {
                    var nextPosts = await _dapperPostRepository.AllHomePosts(query.UserId, nextOffset, limit, query.Date);
                    nextPostId = nextPosts.FirstOrDefault()?.Id;
                    previousPostId = posts[i - 1].Id;
                }
                else
                {
                    nextPostId = posts[i + 1].Id;
                    previousPostId = posts[i - 1].Id;
                }

                currentPost = post;
                break;
            }

            if (currentPost == null)
                return Errors.Common.NotFound;
            
            return new GetHomePostsSliderResult(
                previousPostId,
                nextPostId,
                currentPost
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }
    }
}