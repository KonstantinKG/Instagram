namespace Instagram.Contracts.Post.DeletePostGalleryContracts;

public record DeletePostGalleryRequest(
    Guid Id,
    Guid PostId
    );