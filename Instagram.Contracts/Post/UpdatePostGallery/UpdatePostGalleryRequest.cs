using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.Post.UpdatePostGallery;

public record UpdatePostGalleryRequest(
    Guid id,
    Guid post_id,
    IFormFile file,
    string? description,
    string? labels
    );