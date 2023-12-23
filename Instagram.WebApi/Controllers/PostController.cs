using ErrorOr;

using Instagram.Application.Services.PostService.Commands;
using Instagram.Contracts.Post.CreatePostContracts;

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
    [Route("create")]
    public async Task<IActionResult> Create([FromForm] CreatePostRequest request)
    {
        var createPostCommand = _mapper.Map<CreatePostCommand>(request);
        var handler = HttpContext.RequestServices.GetRequiredService<CreatePostCommandHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<CreatePostCommandPipeline>();
        ErrorOr<CreatePostResult> serviceResult = await pipeline.Pipe(createPostCommand, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<CreatePostResponse>(result)),
            errors => Problem(errors)
        );
    }
}