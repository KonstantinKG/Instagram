namespace Instagram.Contracts.Post.GetPostContracts;

public record GetPostResponse(
    Guid Id,
    string? Content,
    long? LocationId,
    long? Views,
    bool HideStats,
    bool HideComments,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<GetPostGalleryResponse> Galleries
);

public record GetPostGalleryResponse(
    string File,
    string? Description,
    string? Labels
);