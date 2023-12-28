using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Common.Errors;

namespace Instagram.Application.Services.PostService.Commands.DeletePostGallery;

public class DeletePostGalleryCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly IDapperPostRepository _dapperPostRepository;

    public DeletePostGalleryCommandHandler(
        IEfPostRepository efPostRepository,
        IDapperPostRepository dapperPostRepository
        )
    {
        _efPostRepository = efPostRepository;
        _dapperPostRepository = dapperPostRepository;
    }
    
    public async Task<ErrorOr<DeletePostGalleryResult>> Handle(DeletePostGalleryCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var post = await _dapperPostRepository.GetPost(command.PostId);
            if (post == null)
                return Errors.Common.NotFound;

            if (post.UserId != command.UserId)
                return Errors.Common.AccessDenied;

            var gallery = post.Galleries.FirstOrDefault(x => x.Id == command.Id);
            if (gallery == null)
                return Errors.Common.NotFound;

            if (post.Galleries.Count <= 1)
                await _efPostRepository.DeletePost(post);
            else
                await _efPostRepository.DeleteGallery(gallery);

            return new DeletePostGalleryResult();
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }

    }
}