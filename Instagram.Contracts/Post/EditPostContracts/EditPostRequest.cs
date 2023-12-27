namespace Instagram.Contracts.Post.EditPostContracts;

public record EditPostRequest(
    Guid Id,
    string? Content,
    long? LocationId,
    bool HideStats,
    bool HideComments
    );