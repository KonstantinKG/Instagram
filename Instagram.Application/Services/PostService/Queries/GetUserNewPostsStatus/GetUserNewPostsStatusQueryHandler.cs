using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Services.PostService.Queries.GetHomePostsSlider;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.PostService.Queries.GetUserNewPostsStatus;

public class GetUserNewPostsStatusQueryHandler
{
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ILogger<GetUserNewPostsStatusQueryHandler> _logger;

    public GetUserNewPostsStatusQueryHandler(
        IDapperPostRepository dapperUserRepository,
        ILogger<GetUserNewPostsStatusQueryHandler> logger)
    {
        _dapperPostRepository = dapperUserRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<GetUserNewPostsStatusResult>> Handle(GetUserNewPostsStatusQuery query,  CancellationToken cancellationToken)
    {
        try
        {
            var status = await _dapperPostRepository.HasUserNewPosts(query.UserId, query.Date);
            return new GetUserNewPostsStatusResult(status);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }
    }
}