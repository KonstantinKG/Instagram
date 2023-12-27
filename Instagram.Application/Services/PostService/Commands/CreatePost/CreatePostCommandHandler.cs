using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Commands.CreatePost;

public class CreatePostCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;

    public CreatePostCommandHandler(
        IEfPostRepository efPostRepository
        )
    {
        _efPostRepository = efPostRepository;
    }
    
    public async Task<ErrorOr<CreatePostResult>> Handle(CreatePostCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var post = Post.Create(command.UserId);
            await _efPostRepository.AddPost(post);
            
            return new CreatePostResult(
                post.Id
            );
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }

    }
}