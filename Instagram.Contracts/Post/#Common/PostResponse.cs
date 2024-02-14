using Instagram.Contracts.User._Common;

namespace Instagram.Contracts.Post._Common;

public record PostResponse(
    Guid id,
    string? content,
    long? location_id,
    long? views,
    bool? hide_stats,
    bool? hide_comments,
    long? comments_count,
    long? likes_count,
    DateTime? created_at,
    DateTime? updated_at,
    List<PostGalleryResponse> galleries,
    List<PostTagResponse> tags,
    UserResponse user
);