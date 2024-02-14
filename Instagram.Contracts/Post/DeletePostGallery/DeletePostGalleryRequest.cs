namespace Instagram.Contracts.Post.DeletePostGallery;

public record DeletePostGalleryRequest(
    Guid id,
    Guid post_id
);