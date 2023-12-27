using Instagram.Contracts.Post.GetPostContracts;

namespace Instagram.Contracts.Post.GetAllPostsContracts;

public record GetAllPostsResponse(
    List<GetPostResponse> Posts
);