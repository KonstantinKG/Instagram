using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Commands.DeletePost;

public class DeletePostCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly IDapperPostRepository _dapperPostRepository;

    public DeletePostCommandHandler(
        IEfPostRepository efPostRepository,
        IDapperPostRepository dapperPostRepository)
    {
        _efPostRepository = efPostRepository;
        _dapperPostRepository = dapperPostRepository;
    }
    
    public async Task<ErrorOr<DeletePostResult>> Handle(DeletePostCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var post = await _dapperPostRepository.GetPost(command.Id);
            if (post == null)
                return Errors.Common.NotFound;

            if (post.UserId != command.UserId)
                return Errors.Common.AccessDenied;
            
            await _efPostRepository.DeletePost(post);
            
            return new DeletePostResult();
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }

    }
}