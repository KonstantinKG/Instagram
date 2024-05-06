using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.PostService.Commands.AddPostComment;

public class AddPostCommentCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly ILogger<AddPostCommentCommandHandler> _logger;

    public AddPostCommentCommandHandler(
        IEfPostRepository efPostRepository,
        ILogger<AddPostCommentCommandHandler> logger
        )
    {
        _efPostRepository = efPostRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<AddPostCommentResult>> Handle(AddPostCommentCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var comment = new PostComment {
                Id = Guid.NewGuid(),
                PostId = command.PostId,
                ParentId = command.ParentId,
                UserId = command.UserId,
                Content = command.Content
            };
            
            await _efPostRepository.AddComment(comment);

            return new AddPostCommentResult(comment);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }

    }
}