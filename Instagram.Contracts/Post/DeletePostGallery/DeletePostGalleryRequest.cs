namespace Instagram.Contracts.Post.DeletePostGallery;

public record DeletePostGalleryRequest(
    Guid Id,
    Guid PostId
);