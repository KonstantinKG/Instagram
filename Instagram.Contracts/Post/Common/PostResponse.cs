namespace Instagram.Contracts.Post.Common;

public record PostResponse(
    Guid Id,
    string? Content,
    long? LocationId,
    long? Views,
    bool HideStats,
    bool HideComments,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<GalleryResponse> Galleries
);
    