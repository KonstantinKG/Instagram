using ErrorOr;

using Instagram.Application.Services.PostService.Commands.AddPostGallery;
using Instagram.Application.Services.PostService.Commands.ConfirmPost;
using Instagram.Application.Services.PostService.Commands.CreatePost;
using Instagram.Application.Services.PostService.Commands.DeletePost;
using Instagram.Application.Services.PostService.Commands.EditPost;
using Instagram.Application.Services.PostService.Commands.EditPostGallery;
using Instagram.Application.Services.PostService.Queries.GetAllPosts;
using Instagram.Application.Services.PostService.Queries.GetPost;
using Instagram.Contracts.Post.AddPostGalleryContracts;
using Instagram.Contracts.Post.ConfirmPostContracts;
using Instagram.Contracts.Post.CreatePostContracts;
using Instagram.Contracts.Post.DeletePostContracts;
using Instagram.Contracts.Post.EditPostContracts;
using Instagram.Contracts.Post.EditPostGalleryContracts;
using Instagram.Contracts.Post.GetAllPostsContracts;
using Instagram.Contracts.Post.GetPostContracts;

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
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create()
    {
        var userId = Guid.Parse(GetUserClaim<string>(c => c.Type == "nameid"));
        var createPostCommand = new CreatePostCommand(userId);
        var handler = HttpContext.RequestServices.GetRequiredService<CreatePostCommandHandler>();
        ErrorOr<CreatePostResult> serviceResult = await handler.Handle(createPostCommand, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<CreatePostResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    [Route("edit")]
    public async Task<IActionResult> Edit(EditPostRequest request)
    {
        var userId = Guid.Parse(GetUserClaim<string>(c => c.Type == "nameid"));
        var editPostCommand = _mapper.Map<EditPostCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<EditPostCommandHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<EditPostCommandPipeline>();
        ErrorOr<EditPostResult> serviceResult = await pipeline.Pipe(editPostCommand, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> Get([FromQuery] GetPostRequest request)
    {
        var getPostQuery = _mapper.Map<GetPostQuery>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<GetPostQueryHandler>();
        ErrorOr<GetPostResult> serviceResult = await handler.Handle(getPostQuery, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<GetPostResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> All([FromQuery] GetAllPostsRequest request)
    {
        var getAllPostsQuery = _mapper.Map<GetAllPostsQuery>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<GetAllPostsQueryHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<GetAllPostsQueryPipeline>();
        ErrorOr<GetAllPostsResult> serviceResult = await pipeline.Pipe(getAllPostsQuery, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<GetAllPostsResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    [Route("confirm")]
    public async Task<IActionResult> Confirm(ConfirmPostRequest request)
    {
        var userId = Guid.Parse(GetUserClaim<string>(c => c.Type == "nameid"));
        var confirmPostCommand = _mapper.Map<ConfirmPostCommand>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<ConfirmPostCommandHandler>();
        ErrorOr<ConfirmPostResult> serviceResult = await handler.Handle(confirmPostCommand, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(DeletePostRequest request)
    {
        var userId = Guid.Parse(GetUserClaim<string>(c => c.Type == "nameid"));
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
        var userId = Guid.Parse(GetUserClaim<string>(c => c.Type == "nameid"));
        var addPostGalleryCommand = _mapper.Map<AddPostGalleryCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<AddPostGalleryCommandHandler>();
        ErrorOr<AddPostGalleryResult> serviceResult = await handler.Handle(addPostGalleryCommand, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    [Route("gallery/edit")]
    public async Task<IActionResult> GalleryEdit([FromForm] EditPostGalleryRequest request)
    {
        var userId = Guid.Parse(GetUserClaim<string>(c => c.Type == "nameid"));
        var editPostGalleryCommand = _mapper.Map<EditPostGalleryCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<EditPostGalleryCommandHandler>();
        ErrorOr<EditPostGalleryResult> serviceResult = await handler.Handle(editPostGalleryCommand, CancellationToken.None);

        return serviceResult.Match(
            _ => Ok(),
            errors => Problem(errors)
        );
    }
}