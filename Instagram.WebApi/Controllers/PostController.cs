using System.ComponentModel.DataAnnotations;

using ErrorOr;

using Instagram.Application.Services.PostService.Commands;
using Instagram.Contracts.Post.CreatePostContracts;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
    public async Task<IActionResult> Create(CreatePostRequest request)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == "nameid").Value;
        var createPostCommand = _mapper.Map<CreatePostCommand>((userId, request));
        var handler = HttpContext.RequestServices.GetRequiredService<CreatePostCommandHandler>();
        var pipeline = HttpContext.RequestServices.GetRequiredService<CreatePostCommandPipeline>();
        ErrorOr<CreatePostResult> serviceResult = await pipeline.Pipe(createPostCommand, handler.Handle, CancellationToken.None);

        return serviceResult.Match(
            result => Ok(_mapper.Map<CreatePostResponse>(result)),
            errors => Problem(errors)
        );
    }
    
    [HttpPost]
    [Route("create2")]
    public async Task<IActionResult> Create2([FromForm] List<CreatePostRequestImag2e> request)
    {
        await Task.CompletedTask;
        return Ok();
    }
}