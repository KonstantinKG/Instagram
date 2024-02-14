using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.Post.AddPostGallery;

public record AddPostGalleryRequest(
    Guid post_id,
    IFormFile file,
    string? description,
    string? labels
    );