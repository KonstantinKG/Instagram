using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.PostService.Commands.AddPost;

public class AddPostCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly ILogger<AddPostCommandHandler> _logger;

    public AddPostCommandHandler(
        IEfPostRepository efPostRepository, 
        ILogger<AddPostCommandHandler> logger
        )
    {
        _efPostRepository = efPostRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<AddPostResult>> Handle(AddPostCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var post = Post.Create(command.UserId);
            await _efPostRepository.AddPost(post);
            
            return new AddPostResult(
                post.Id
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }

    }
}