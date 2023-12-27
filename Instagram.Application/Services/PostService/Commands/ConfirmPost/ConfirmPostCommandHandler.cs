using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Commands.ConfirmPost;

public class ConfirmPostCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly IDapperPostRepository _dapperPostRepository;

    public ConfirmPostCommandHandler(
        IEfPostRepository efPostRepository,
        IDapperPostRepository dapperPostRepository)
    {
        _efPostRepository = efPostRepository;
        _dapperPostRepository = dapperPostRepository;
    }
    
    public async Task<ErrorOr<ConfirmPostResult>> Handle(ConfirmPostCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var post = await _dapperPostRepository.GetPost(command.Id);
            if (post == null)
                return Errors.Common.NotFound;

            if (post.UserId != command.UserId)
                return Errors.Common.AccessDenied;

            var confirmedPost = Post.Fill(
                post.Id,
                post.UserId,
                post.Content,
                post.LocationId,
                post.Views,
                post.HideStats,
                post.HideComments,
                true
            );

            await _efPostRepository.UpdatePost(confirmedPost);
            
            return new ConfirmPostResult();
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }

    }
}