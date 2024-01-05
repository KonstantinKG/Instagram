using ErrorOr;

using Instagram.Application.Services.PostService.Commands.AddPost;
using Instagram.Application.Services.PostService.Commands.AddPostComment;
using Instagram.Application.Services.PostService.Commands.AddPostGallery;
using Instagram.Application.Services.PostService.Commands.DeletePost;
using Instagram.Application.Services.PostService.Commands.DeletePostComment;
using Instagram.Application.Services.PostService.Commands.DeletePostGallery;
using Instagram.Application.Services.PostService.Commands.UpdatePost;
using Instagram.Application.Services.PostService.Commands.UpdatePostComment;
using Instagram.Application.Services.PostService.Commands.UpdatePostGallery;
using Instagram.Application.Services.PostService.Commands.UpdatePostStatus;
using Instagram.Application.Services.PostService.Queries.AllHomePosts;
using Instagram.Application.Services.PostService.Queries.AllPostCommentChildren;
using Instagram.Application.Services.PostService.Queries.AllPostComments;
using Instagram.Application.Services.PostService.Queries.AllPostParentComments;
using Instagram.Application.Services.PostService.Queries.AllPosts;
using Instagram.Application.Services.PostService.Queries.AllUserPosts;
using Instagram.Application.Services.PostService.Queries.GetHomeNewPostsStatus;
using Instagram.Application.Services.PostService.Queries.GetHomePostsSlider;
using Instagram.Application.Services.PostService.Queries.GetPost;
using Instagram.Application.Services.PostService.Queries.GetUserNewPostsStatus;
using Instagram.Application.Services.PostService.Queries.GetUserPostsSlider;
using Instagram.Contracts.Post._Common;
using Instagram.Contracts.Post.AddPost;
using Instagram.Contracts.Post.AddPostComment;
using Instagram.Contracts.Post.AddPostGallery;
using Instagram.Contracts.Post.AllHomePosts;
using Instagram.Contracts.Post.AllPostCommentChildren;
using Instagram.Contracts.Post.AllPostComments;
using Instagram.Contracts.Post.AllPostParentComments;
using Instagram.Contracts.Post.AllPosts;
using Instagram.Contracts.Post.AllUserPosts;
using Instagram.Contracts.Post.DeletePost;
using Instagram.Contracts.Post.DeletePostComment;
using Instagram.Contracts.Post.DeletePostGallery;
using Instagram.Contracts.Post.GetHomeNewPostsStatus;
using Instagram.Contracts.Post.GetHomePostsSlider;
using Instagram.Contracts.Post.GetPost;
using Instagram.Contracts.Post.GetUserNewPostsStatus;
using Instagram.Contracts.Post.GetUserPostsSlider;
using Instagram.Contracts.Post.UpdatePost;
using Instagram.Contracts.Post.UpdatePostComment;
using Instagram.Contracts.Post.UpdatePostGallery;
using Instagram.Contracts.Post.UpdatePostStatus;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc;

namespace Instagram.WebApi.Controllers;

[Route("post")]
public class PostController : ApiController
{
    private readonly IMapper _mapper;

    public PostController(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> Get([FromQuery] GetPostRequest request)
    {
        var getPostQuery = new GetPostQuery(request.Id);
        var handler = HttpContext.RequestServices.GetRequiredService<GetPostQueryHandler>();
        ErrorOr<GetPostResult> serviceResult = await handler.Handle(getPostQuery, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<PostResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> All([FromQuery] AllPostsRequest request)
    {
        var allPostsQuery = _mapper.Map<AllPostsQuery>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<AllPostsQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<AllPostsQueryPipeline>();
        ErrorOr<AllPostsResult> serviceResult = await pipeline.Pipe(allPostsQuery, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AllPostsResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("user/all")]
    public async Task<IActionResult> GetAllUserPosts([FromQuery] AllUserPostsRequest request)
    {
        
        var userId = request.UserId ?? GetUserId();
        var allPostsOfUserQuery = _mapper.Map<AllUserPostsQuery>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<AllUserPostsQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<AllUserPostsQueryPipeline>();
        ErrorOr<AllUserPostsResult> serviceResult = await pipeline.Pipe(allPostsOfUserQuery, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AllUserPostsResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("user/slider")]
    public async Task<IActionResult> GetUserPostsSlider([FromQuery] GetUserPostsSliderRequest request)
    {
        var userId = request.UserId ?? GetUserId();
        var getPostsOfUserSliderQuery =_mapper.Map<GetUserPostsSliderQuery>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<GetUserPostsSliderQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<GetUserPostsSliderQueryPipeline>();
        ErrorOr<GetUserPostsSliderResult> serviceResult = await pipeline.Pipe(getPostsOfUserSliderQuery, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<GetUserPostsSliderResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("user/new")]
    public async Task<IActionResult> GetUserNewPostsStatus([FromQuery] GetUserNewPostsStatusRequest request)
    {
        var userId = GetUserId();
        var getPostsOfUserNewStatusQuery =_mapper.Map<GetUserNewPostsStatusQuery>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<GetUserNewPostsStatusQueryHandler>();
        ErrorOr<GetUserNewPostsStatusResult> serviceResult = await handler.Handle(getPostsOfUserNewStatusQuery, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<GetUserNewPostsStatusResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("home/all")]
    public async Task<IActionResult> GetAllHomePosts([FromQuery] AllHomePostsRequest request)
    {
        var userId = GetUserId();
        var query = _mapper.Map<AllHomePostsQuery>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<AllHomePostsQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<AllHomePostsQueryPipeline>();
        ErrorOr<AllHomePostsResult> serviceResult = await pipeline.Pipe(query, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AllHomePostsResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("home/slider")]
    public async Task<IActionResult> GetHomePostsSlider([FromQuery] GetHomePostsSliderRequest request)
    {
        var userId = GetUserId();
        var query =_mapper.Map<GetHomePostsSliderQuery>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<GetHomePostsSliderQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<GetHomePostsSliderQueryPipeline>();
        ErrorOr<GetHomePostsSliderResult> serviceResult = await pipeline.Pipe(query, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<GetHomePostsSliderResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("home/new")]
    public async Task<IActionResult> GetHomeNewPostsStatus([FromQuery] GetHomeNewPostsStatusRequest request)
    {
        var query = _mapper.Map<GetHomeNewPostsStatusQuery>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<GetHomeNewPostsStatusQueryHandler>();
        ErrorOr<GetHomeNewPostsStatusResult> serviceResult = await handler.Handle(query, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<GetHomeNewPostsStatusResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add()
    {
        var userId = GetUserId();
        var createPostCommand = new AddPostCommand(userId);
        var handler = HttpContext.RequestServices.GetRequiredService<AddPostCommandHandler>();
        ErrorOr<AddPostResult> serviceResult = await handler.Handle(createPostCommand, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AddPostResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update(UpdatePostRequest request)
    {
        var userId = GetUserId();
        var editPostCommand = _mapper.Map<EditPostCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<EditPostCommandHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<EditPostCommandPipeline>();
        ErrorOr<EditPostResult> serviceResult = await pipeline.Pipe(editPostCommand, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpPut]
    [Route("publish")]
    public async Task<IActionResult> Publish(UpdatePostStatusRequest request)
    {
        var userId = GetUserId();
        var confirmPostCommand = _mapper.Map<UpdatePostStatusCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<UpdatePostStatusCommandHandler>();
        ErrorOr<UpdatePostStatusResult> serviceResult = await handler.Handle(confirmPostCommand, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> Delete(DeletePostRequest request)
    {
        var userId = GetUserId();
        var deletePostCommand = _mapper.Map<DeletePostCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<DeletePostCommandHandler>();
        ErrorOr<DeletePostResult> serviceResult = await handler.Handle(deletePostCommand, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    [Route("gallery/add")]
    public async Task<IActionResult> GalleryAdd([FromForm] AddPostGalleryRequest request)
    {
        var userId = GetUserId();
        var addPostGalleryCommand = _mapper.Map<AddPostGalleryCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<AddPostGalleryCommandHandler>();
        ErrorOr<AddPostGalleryResult> serviceResult = await handler.Handle(addPostGalleryCommand, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpPut]
    [Route("gallery/update")]
    public async Task<IActionResult> GalleryUpdate([FromForm] UpdatePostGalleryRequest request)
    {
        var userId = GetUserId();
        var editPostGalleryCommand = _mapper.Map<UpdatePostGalleryCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<UpdatePostGalleryCommandHandler>();
        ErrorOr<UpdatePostGalleryResult> serviceResult = await handler.Handle(editPostGalleryCommand, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpDelete]
    [Route("gallery/delete")]
    public async Task<IActionResult> GalleryDelete(DeletePostGalleryRequest request)
    {
        var userId = GetUserId();
        var deletePostGalleryCommand = _mapper.Map<DeletePostGalleryCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<DeletePostGalleryCommandHandler>();
        ErrorOr<DeletePostGalleryResult> serviceResult = await handler.Handle(deletePostGalleryCommand, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("comment/all")]
    public async Task<IActionResult> CommentAll([FromQuery] AllPostCommentsRequest request)
    {
        var query = _mapper.Map<AllPostCommentsQuery>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<AllPostCommentsQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<AllPostCommentsQueryPipeline>();
        ErrorOr<AllPostCommentsResult> serviceResult = await pipeline.Pipe(query, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AllPostCommentsResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("comment/all/parents")]
    public async Task<IActionResult> CommentAllParents([FromQuery] AllPostParentCommentsRequest request)
    {
        var userId = GetUserId();
        var query = _mapper.Map<AllPostParentCommentsQuery>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<AllPostParentCommentsQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<AllPostParentCommentsQueryPipeline>();
        ErrorOr<AllPostParentCommentsResult> serviceResult = await pipeline.Pipe(query, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AllPostParentCommentsResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("comment/all/children")]
    public async Task<IActionResult> CommentAllChildren([FromQuery] AllPostCommentChildrenRequest request)
    {
        var userId = GetUserId();
        var query = _mapper.Map<AllPostCommentChildrenQuery>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<AllPostCommentChildrenQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<AllPostCommentChildrenQueryPipeline>();
        ErrorOr<AllPostCommentChildrenResult> serviceResult = await pipeline.Pipe(query, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<AllPostCommentChildrenResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    [Route("comment/add")]
    public async Task<IActionResult> CommentAdd([FromForm] AddPostCommentRequest request)
    {
        var userId = GetUserId();
        var addPostCommentCommand = _mapper.Map<AddPostCommentCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<AddPostCommentCommandHandler>();
        ErrorOr<AddPostCommentResult> serviceResult = await handler.Handle(addPostCommentCommand, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpPut]
    [Route("comment/update")]
    public async Task<IActionResult> CommentUpdate([FromForm] UpdatePostCommentRequest request)
    {
        var userId = GetUserId();
        var editPostCommentCommand = _mapper.Map<UpdatePostCommentCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<UpdatePostCommentCommandHandler>();
        ErrorOr<UpdatePostCommentResult> serviceResult = await handler.Handle(editPostCommentCommand, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpDelete]
    [Route("comment/delete")]
    public async Task<IActionResult> CommentDeelete(DeletePostCommentRequest request)
    {
        var userId = GetUserId();
        var deletePostCommentCommand = _mapper.Map<DeletePostCommentCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<DeletePostCommentCommandHandler>();
        ErrorOr<DeletePostCommentResult> serviceResult = await handler.Handle(deletePostCommentCommand, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
}