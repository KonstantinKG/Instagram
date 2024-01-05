namespace Instagram.Contracts.Post._Common;

public record PostShortResponse(
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
    List<PostGalleryResponse> Galleries
);
    