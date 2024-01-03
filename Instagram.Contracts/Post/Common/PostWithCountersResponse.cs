namespace Instagram.Contracts.Post.Common;

public record PostWithCountersResponse(
    Guid Id,
    string? Content,
    long? LocationId,
    long? Views,
    long CommentsCount,
    long LikesCount,
    bool HideStats,
    bool HideComments,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<GalleryResponse> Galleries
);
    