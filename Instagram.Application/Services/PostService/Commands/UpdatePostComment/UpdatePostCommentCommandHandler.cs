using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.PostService.Commands.UpdatePostComment;

public class UpdatePostCommentCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ILogger<UpdatePostCommentCommandHandler> _logger;

    public UpdatePostCommentCommandHandler(
        IEfPostRepository efPostRepository,
        IDapperPostRepository dapperPostRepository,
        ILogger<UpdatePostCommentCommandHandler> logger
        )
    {
        _efPostRepository = efPostRepository;
        _dapperPostRepository = dapperPostRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<UpdatePostCommentResult>> Handle(UpdatePostCommentCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var comment = await _dapperPostRepository.GetComment(command.Id);
            if (comment == null)
                return Errors.Common.NotFound;

            if (comment.UserId != command.UserId)
                return Errors.Common.AccessDenied;
            
            var updatedComment = new PostComment {
                Id = comment.Id,
                PostId = comment.PostId,
                ParentId = comment.ParentId,
                UserId = comment.UserId,
                Content = command.Content
            };
            
            if (updatedComment.Different(comment))
                await _efPostRepository.UpdateComment(updatedComment);

            return new UpdatePostCommentResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }

    }
}