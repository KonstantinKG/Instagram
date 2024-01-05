namespace Instagram.Contracts.Post._Common;


public record PostGalleryResponse(
    Guid Id,
    string File,
    string? Description,
    string? Labels
);