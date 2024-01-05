using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Common.Exceptions;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.PostService.Commands.AddPostGallery;

public class AddPostGalleryCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly IFileDownloader _fileDownloader;
    private readonly ILogger<AddPostGalleryCommandHandler> _logger;

    public AddPostGalleryCommandHandler(
        IEfPostRepository efPostRepository,
        IDapperPostRepository dapperPostRepository,
        IFileDownloader fileDownloader, 
        ILogger<AddPostGalleryCommandHandler> logger)
    {
        _efPostRepository = efPostRepository;
        _dapperPostRepository = dapperPostRepository;
        _fileDownloader = fileDownloader;
        _logger = logger;
    }
    
    public async Task<ErrorOr<AddPostGalleryResult>> Handle(AddPostGalleryCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var post = await _dapperPostRepository.GetPost(command.PostId);
            if (post == null)
                return Errors.Common.NotFound;

            if (post.UserId != command.UserId)
                return Errors.Common.AccessDenied;

            var path = await _fileDownloader.Download(command.File, "post_galleries");
            var gallery = new PostGallery {
                Id = Guid.NewGuid(),
                PostId = command.PostId,
                File = path,
                Description = command.Description,
                Labels = command.Labels
            };
            await _efPostRepository.AddGallery(gallery);

            return new AddPostGalleryResult();
        }
        catch (FileSaveException e)
        {
            _logger.LogError(e, "Saving file '{Filename}' failed in {Name}", command.File.FileName(), GetType().Name);
            return Error.Failure(string.Format(Errors.File.DownloadFailed.Code, command.File.FileName()));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }

    }
}