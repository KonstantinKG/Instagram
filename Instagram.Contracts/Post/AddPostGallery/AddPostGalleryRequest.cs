using Microsoft.AspNetCore.Http;

namespace Instagram.Contracts.Post.AddPostGallery;

public record AddPostGalleryRequest(
    Guid PostId,
    IFormFile File,
    string? Description,
    string? Labels
    );