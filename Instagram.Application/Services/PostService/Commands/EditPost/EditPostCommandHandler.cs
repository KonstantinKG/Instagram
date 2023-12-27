using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Commands.EditPost;

public class EditPostCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly IDapperPostRepository _dapperPostRepository;

    public EditPostCommandHandler(
        IEfPostRepository efPostRepository, 
        IDapperPostRepository dapperPostRepository
        )
    {
        _efPostRepository = efPostRepository;
        _dapperPostRepository = dapperPostRepository;
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
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }

    }
}