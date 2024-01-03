namespace Instagram.Contracts.Post.UpdatePost;

public record UpdatePostRequest(
    Guid Id,
    string? Content,
    long? LocationId,
    bool HideStats,
    bool HideComments
    );