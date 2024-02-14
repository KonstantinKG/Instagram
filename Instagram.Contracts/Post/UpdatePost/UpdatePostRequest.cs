namespace Instagram.Contracts.Post.UpdatePost;

public record UpdatePostRequest(
    Guid id,
    string? content,
    long? location_id,
    bool hide_stats,
    bool hide_comments,
    List<string> tags
    );