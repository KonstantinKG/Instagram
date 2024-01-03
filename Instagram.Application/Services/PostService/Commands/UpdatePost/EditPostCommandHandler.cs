using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.PostService.Commands.UpdatePost;

public class EditPostCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ILogger<EditPostCommandHandler> _logger;

    public EditPostCommandHandler(
        IEfPostRepository efPostRepository, 
        IDapperPostRepository dapperPostRepository, 
        ILogger<EditPostCommandHandler> logger)
    {
        _efPostRepository = efPostRepository;
        _dapperPostRepository = dapperPostRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<EditPostResult>> Handle(EditPostCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var post = await _dapperPostRepository.GetPost(command.Id);
            if (post is null)
                return Errors.Common.NotFound;

            if (post.UserId != command.UserId)
                return Errors.Common.AccessDenied;
            
            var updatedPost = Post.Fill(
                command.Id,
                command.UserId,
                command.Content,
                command.LocationId == 0 ? null : command.LocationId,
                post.Views,
                command.HideStats,
                command.HideComments,
                post.Active
            );
            
            if (updatedPost.Different(post))
                await _efPostRepository.UpdatePost(post);
            
            return new EditPostResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }

    }
}