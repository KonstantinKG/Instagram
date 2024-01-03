using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.PostService.Queries.GetPost;

public class GetPostQueryHandler
{
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ILogger<GetPostQueryHandler> _logger;

    public GetPostQueryHandler(
        IDapperPostRepository dapperUserRepository,
        ILogger<GetPostQueryHandler> logger)
    {
        _dapperPostRepository = dapperUserRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<GetPostResult>> Handle(GetPostQuery query, CancellationToken cancellationToken)
    {
        try
        {
            if (await _dapperPostRepository.GetPostWithGallery(query.Id) is not Post post)
            {
                return Errors.Common.NotFound;
            }
            return new GetPostResult(post);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }
    }
}