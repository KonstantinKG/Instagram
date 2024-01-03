using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.PostService.Commands.DeletePostComment;

public class DeletePostCommentCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ILogger<DeletePostCommentCommandHandler> _logger;

    public DeletePostCommentCommandHandler(
        IEfPostRepository efPostRepository,
        IDapperPostRepository dapperPostRepository, 
        ILogger<DeletePostCommentCommandHandler> logger)
    {
        _efPostRepository = efPostRepository;
        _dapperPostRepository = dapperPostRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<DeletePostCommentResult>> Handle(DeletePostCommentCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var comment = await _dapperPostRepository.GetComment(command.Id);
            if (comment == null)
                return Errors.Common.NotFound;

            if (comment.UserId != command.UserId)
                return Errors.Common.AccessDenied;

            await _efPostRepository.DeleteComment(comment);

            return new DeletePostCommentResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }

    }
}