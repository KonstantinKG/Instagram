using Instagram.Contracts.User._Common;

namespace Instagram.Contracts.Post._Common;

public record PostResponse(
    Guid Id,
    string? Content,
    long? LocationId,
    long? Views,
    bool HideStats,
    bool HideComments,
    long CommentsCount,
    long LikesCount,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<PostGalleryResponse> Galleries,
    List<PostTagResponse> Tags,
    UserShortResponse User
);
    