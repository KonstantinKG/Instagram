using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.PostService.Commands.UpdatePostStatus;

public class UpdatePostStatusCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ILogger<UpdatePostStatusCommandHandler> _logger;

    public UpdatePostStatusCommandHandler(
        IEfPostRepository efPostRepository,
        IDapperPostRepository dapperPostRepository, 
        ILogger<UpdatePostStatusCommandHandler> logger)
    {
        _efPostRepository = efPostRepository;
        _dapperPostRepository = dapperPostRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<UpdatePostStatusResult>> Handle(UpdatePostStatusCommand statusCommand,  CancellationToken cancellationToken)
    {
        try
        {
            var post = await _dapperPostRepository.GetPostWithGalleries(statusCommand.Id);
            if (post == null)
                return Errors.Common.NotFound;

            if (post.UserId != statusCommand.UserId)
                return Errors.Common.AccessDenied;

            if (post.Galleries.Count == 0)
                return Errors.Post.GalleriesNotFound;
            
            var confirmedPost = new Post {
                Id = post.Id,
                UserId = post.UserId,
                Content = post.Content,
                LocationId = post.LocationId,
                Views = post.Views,
                HideStats = post.HideStats,
                HideComments = post.HideComments,
                CreatedAt = post.CreatedAt,
                Active = true
            };

            await _efPostRepository.UpdatePost(confirmedPost);
            
            return new UpdatePostStatusResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }

    }
}