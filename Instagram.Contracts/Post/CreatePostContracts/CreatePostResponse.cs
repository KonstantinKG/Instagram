using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.Post.CreatePostContracts;

public record CreatePostResponse(
    string PostId
    );