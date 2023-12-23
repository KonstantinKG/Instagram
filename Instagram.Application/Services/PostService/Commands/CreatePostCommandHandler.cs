using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.UserAggregate.ValueObjects;
using Instagram.Domain.Common.Errors;
using Instagram.Domain.Common.Exceptions;

namespace Instagram.Application.Services.PostService.Commands;

public class CreatePostCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly IFileDownloader _fileDownloader;

    public CreatePostCommandHandler(
        IEfPostRepository efPostRepository,
        IFileDownloader fileDownloader)
    {
        _efPostRepository = efPostRepository;
        _fileDownloader = fileDownloader;
    }
    
    public async Task<ErrorOr<CreatePostResult>> Handle(CreatePostCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var post = Post.Create(
                UserId.Fill(Guid.Parse(command.UserId)),
                command.Content,
                command.LocationId,
                null,
                command.HideStats,
                command.HideComments
            );

            var galleries = new List<PostGallery>();
            foreach (var image in command.Images)
            {
                var file = await _fileDownloader.Download(image.Image, "posts");
                var gallery = PostGallery.Create(
                    file,
                    image.Description,
                    image.Labels
                );
                
                galleries.Add(gallery);
            }
            
            await _efPostRepository.AddPost(post, galleries);
            
            return new CreatePostResult(post.Id.Value.ToString());
        }
        catch (FileDownloadException e)
        {
            return Error.Failure(code: $"{Errors.File.DownloadFailed.Code}:{e.Message}");
        }
        catch (Exception)
        {
            return Errors.Common.Unexpected;
        }

    }
}