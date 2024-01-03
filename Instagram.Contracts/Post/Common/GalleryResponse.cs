namespace Instagram.Contracts.Post.Common;


public record GalleryResponse(
    Guid Id,
    string File,
    string? Description,
    string? Labels
);