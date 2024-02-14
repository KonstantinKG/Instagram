namespace Instagram.Contracts.Post._Common;


public record PostGalleryResponse(
    Guid id,
    string file,
    string? description,
    string? labels
);