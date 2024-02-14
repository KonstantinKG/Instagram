using ErrorOr;

using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Domain.Aggregates.PostAggregate;
using Instagram.Domain.Aggregates.PostAggregate.Entities;
using Instagram.Domain.Aggregates.TagAggregate;
using Instagram.Domain.Common.Errors;

using Microsoft.Extensions.Logging;

namespace Instagram.Application.Services.PostService.Commands.UpdatePost;

public class UpdatePostCommandHandler
{
    private readonly IEfPostRepository _efPostRepository;
    private readonly IDapperPostRepository _dapperPostRepository;
    private readonly ILogger<UpdatePostCommandHandler> _logger;

    public UpdatePostCommandHandler(
        IEfPostRepository efPostRepository, 
        IDapperPostRepository dapperPostRepository, 
        ILogger<UpdatePostCommandHandler> logger)
    {
        _efPostRepository = efPostRepository;
        _dapperPostRepository = dapperPostRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<bool>> Handle(UpdatePostCommand command,  CancellationToken cancellationToken)
    {
        try
        {
            var post = await _dapperPostRepository.GetPost(command.Id);
            if (post is null)
                return Errors.Common.NotFound;

            if (post.UserId != command.UserId)
                return Errors.Common.AccessDenied;

            var updatedPost = new Post
            {
                Id = command.Id,
                UserId = command.UserId,
                Content = command.Content,
                LocationId = command.LocationId == 0 ? null : command.LocationId,
                Views = post.Views,
                HideStats = command.HideStats,
                HideComments = command.HideComments,
                Active = post.Active,
                CreatedAt = post.CreatedAt
            };
            
            if (updatedPost.Different(post))
                await _efPostRepository.UpdatePost(updatedPost);

            await HandlePostTags(command);
            
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred in {Name}", GetType().Name);
            return Errors.Common.Unexpected;
        }

    }

    private async Task HandlePostTags(UpdatePostCommand command)
    {
        var currentPostTags = await _dapperPostRepository.GetPostTags(command.Id);
        var potentialNewPostTags = command.Tags.ToList();
        var oldPostTags = new List<PostToTag>();

        currentPostTags.ForEach(currentTag =>
        {
            var foundTag = command.Tags.Any(newTag => newTag == currentTag.Name);
            if (!foundTag)
            {
                oldPostTags.Add(new PostToTag
                {
                    PostId = command.Id,
                    TagId = currentTag.Id
                });
            }
            else
                potentialNewPostTags.Remove(currentTag.Name);
        });

        var newTags = new List<Tag>();
        var newPostTags = new List<PostToTag>();
        potentialNewPostTags.ForEach(async name =>
        {
            var tag = await _dapperPostRepository.GetTag(name);
            if (tag == null)
            {
                tag = new Tag { Id = Guid.NewGuid(), Name = name };
                newTags.Add(tag);    
            }
            
            newPostTags.Add(new PostToTag
            {
                PostId = command.Id,
                TagId = tag.Id
            });
        });
            
        await _efPostRepository.DeletePostTags(oldPostTags);
        await _efPostRepository.AddTags(newTags);
        await _efPostRepository.AddPostTags(newPostTags);
    }
}