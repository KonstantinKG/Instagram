using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Application.Services.PostService.Commands.EditPostGallery;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Common.Exceptions;

namespace Instagram.Application.Services.PostService.Commands.EditPostGallery;

public class EditPostGalleryCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly IFileDownloader _fileDownloader;

    public EditPostGalleryCommandHandler(
        IEfPostRepository efPostRepository,
        IDapperPostRepository dapperPostRepository,
        IFileDownloader fileDownloader)
    {
        _efPostRepository = efPostRepository;
        _dapperPostRepository = dapperPostRepository;
        _fileDownloader = fileDownloader;
    }
    
    public async Task<ErrorOr<EditPostGalleryResult>> Handle(EditPostGalleryCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var post = await _dapperPostRepository.GetPost(command.PostId);
            if (post == null)
                return Errors.Common.NotFound;

            if (post.UserId != command.UserId)
                return Errors.Common.AccessDenied;

            var gallery = await _dapperPostRepository.GetGallery(command.Id);
            if (gallery == null)
                return Errors.Common.NotFound;
            
            var path = await _fileDownloader.Download(command.File, "post_galleries");
            var updatedGallery = PostGallery.Create(
                command.PostId,
                path,
                command.Description,
                command.Labels
            );
            
            if (updatedGallery.Different(gallery))
                await _efPostRepository.UpdateGallery(gallery);

            return new EditPostGalleryResult();
        }
        catch (FileDownloadException)
        {
            return Error.Failure(string.Format(Errors.File.DownloadFailed.Code, command.File.FileName()));
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }

    }
}