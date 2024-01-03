using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Services.PostService.Queries.GetUserNewPostsStatus;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.PostService.Queries.GetHomeNewPostsStatus;

public class GetHomeNewPostsStatusQueryHandler
{
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ILogger<GetHomeNewPostsStatusQueryHandler> _logger;

    public GetHomeNewPostsStatusQueryHandler(
        IDapperPostRepository dapperUserRepository,
        ILogger<GetHomeNewPostsStatusQueryHandler> logger)
    {
        _dapperPostRepository = dapperUserRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<GetHomeNewPostsStatusResult>> Handle(GetHomeNewPostsStatusQuery query,  CancellationToken cancellationToken)
    {
        try
        {
            var status = await _dapperPostRepository.HasHomeNewPosts(query.Date);
            return new GetHomeNewPostsStatusResult(status);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }
    }
}