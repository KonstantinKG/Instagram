using Instagram.Contracts.Post.GetPostContracts;

namespace Instagram.Contracts.Post.GetAllPostsContracts;

public record GetAllPostsResponse(
    long Current,
    long Total,
    List<GetPostResponse> Posts
);